using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviour
{
    public enum movementStyle : byte
    {
        Still = 0,
        Creature = 1,
        ChessPiece = 2,
        Car = 4
    }
    
    // player's data
    private global.playerType playerType;
    public string playerName;
    private int number;
    public int playerNumber { get { return number; } set { number = value; } }
    private char ABCD;
    
    public bool isCampaignHero
    { 
        get { return ((!global.clashMode) && (playerNumber == 1)); }
    }    
    
    public bool isMachine
    {
        get { return (playerType == global.playerType.Machine); }
    }
    
    // player's physics
    public float mass = 1.0f;
    public float drag = 5.0f;
    private Rigidbody body;
	public float speed = 80.0f;
	public float dashSpeed = 200.0f;
	public movementStyle movements = movementStyle.Creature;
	private Quaternion defaultRotation;
	private Animator animator;
	private bool hasAnimator;
	private bool groundless;
	private bool dashing;
    public bool isDashing { get { return dashing; } }    
    public bool isMoving { get { return (body.velocity.magnitude > 0.1); } }
    private System.DateTime burningUntil;
    public bool isBurning { get { return (global.now > burningUntil); } }
    
    private string lastAnimationBool;
    
    public bool isGroundless
    { 
        get { return groundless; }
        set { groundless = value; }
    }
    
    public bool isStanding
    {
        get 
        { 
            return ((transform.rotation.eulerAngles.x == defaultRotation.eulerAngles.x) && 
                    (transform.rotation.eulerAngles.z == defaultRotation.eulerAngles.z));
        }
    }
	
	// sounds
	public AudioClip mediumHitSound;
	public AudioClip heavyHitSound;
    
    private void setUpRigidbody()
    {
        body = GetComponent<Rigidbody>();
        body.mass = mass;
        body.drag = drag;
        body.angularDrag = 0.05f;
        body.useGravity = true;
        body.isKinematic = false;
        body.constraints = RigidbodyConstraints.FreezeRotationX | 
                           RigidbodyConstraints.FreezeRotationZ ;
        if (movements == movementStyle.ChessPiece)
            body.constraints |= RigidbodyConstraints.FreezeRotationY;
    }
    
    public void tryAnimate( string animation, string animationBool = "" )
    {
	    if (hasAnimator) 
	    {
	        if (lastAnimationBool != "") animator.SetBool(lastAnimationBool, false);
	        lastAnimationBool = animationBool;
            animator.Play(animation);
	        if (animationBool != "") animator.SetBool(animationBool, true);
	    }
    }
    
    private void delayedStart()
    {
	    if (global.clashMode) 
	    {
	        playerType = global.playerTypes[playerNumber - 1];
	        playerName = global.playerNames[playerNumber - 1];
	        mass = body.mass = (mass <= 2.0f) ? mass : 2.0f;
	        drag = body.drag = (drag <= 5.0f) ? drag : 5.0f;
	    }
	    else if (playerNumber == 1) playerType = global.playerType.Human;
	    else playerType = global.playerType.Machine;
	    if (!((playerNumber > 0) && (playerNumber < 5))) ABCD = 'A';
	    else ABCD = (char)('@'+playerNumber);
	    
	    setUpRigidbody();
	    defaultRotation = transform.rotation;
		groundless = false;
        
        animator = GetComponent<Animator>();
        hasAnimator = (animator != null) ? true : false;
	    
	    if (playerType == global.playerType.Machine)
	        gameObject.AddComponent<AI>();
	    else if (playerType == global.playerType.BrainDead)
	        movements = movementStyle.Still;
	        
	    burningUntil = global.now.AddSeconds(-1);
    }
    
	public void Start() 
	{
	    Invoke("delayedStart", 2.0f);
	}
	
	private void carControl( float horizontal, float vertical )
	{
	    if (isMoving) transform.Rotate(Vector3.up, horizontal * speed * Time.deltaTime);
        if (groundless) return;
        if (vertical != 0.0f) vertical = (vertical > 0.0f) ? 0.25f : -0.25f;
        body.velocity = transform.forward * speed * vertical;
		if (Input.GetButton("dash" + ABCD) && isMoving)
		{
		    body.velocity = transform.forward * dashSpeed * vertical;
		    dashing = true;
		}
		else dashing = false;
	}
	
	private void creatureControl( float horizontal, float vertical )
	{
	    transform.Rotate(Vector3.up, horizontal * speed * 2.0f * Time.deltaTime);
        if (groundless) return;
        if (vertical > 0.0f)
        {
            if (!dashing) tryAnimate("Running", "forward");
            vertical = 0.25f;
        }
        else if (vertical < 0.0f)
        {
            if (!dashing) tryAnimate("RunningBackward", "backward");
            vertical = -0.25f;
            horizontal *= -1.0f;
        }
        body.velocity = transform.forward * speed * vertical;
		if (Input.GetButton("dash" + ABCD) && isMoving && (vertical > 0.0f))
		{
            if (!dashing) tryAnimate("Dash", "dash");
		    body.velocity = transform.forward * dashSpeed * vertical;
		    dashing = true;
		}
		else dashing = false;
	}
	
	private void chessControl( float horizontal, float vertical )
	{
        if (groundless) return;
        if ((horizontal != 0.0f) && (vertical != 0.0f))
        {
            horizontal *= 0.75f;
            vertical *= 0.75f;
        }
        Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);
        body.AddForce(movement * speed);
        if (Input.GetButton("dash" + ABCD) && isMoving)
    	{
    	    body.AddForce(movement * dashSpeed);
    		dashing = true;
    	}
    	else dashing = false;
	}
	
	public void control( float horizontal = 0.0f, float vertical = 0.0f )
	{
	    float h = ((horizontal != 0.0f) || isMachine) ? 
	                horizontal : Input.GetAxis("horizontal" + ABCD);
	    float v = ((vertical != 0.0f) || isMachine) ? 
	                vertical : Input.GetAxis("vertical" + ABCD);
	    
	    if ((h == 0.0f) && (v == 0.0f)) return;
	    else if (movements == movementStyle.Creature) creatureControl(h, v);
	    else if (movements == movementStyle.ChessPiece) chessControl(h, v);
	    else if (movements == movementStyle.Car) carControl(h, v);
	}
	
	private void dealWithRotations()
	{
        if (!global.currentArena.isInsideArenaLimits(transform.position))
            body.constraints = RigidbodyConstraints.None;
        else if (isStanding)
        {
            body.constraints = RigidbodyConstraints.FreezeRotationX | 
                               RigidbodyConstraints.FreezeRotationZ ; 
            if (movements == movementStyle.ChessPiece)
                body.constraints |= RigidbodyConstraints.FreezeRotationY;
        }
	}
	
    public void FixedUpdate()
    {
        if (!global.ongoingGame) return;
        
	    if (body.drag <= 0) body.AddForce(Physics.gravity * body.mass * 2);
	    else if (isBurning && global.chance(0.33f))
	    {
            float x = ((global.coinflip) ? -1.0f : 1.0f) * global.difficultyFactor;
            float y = ((global.coinflip) ? -1.0f : 1.0f) * global.difficultyFactor;
            control(x, y);
        }
	       
	    control();
	    if (!isMoving) tryAnimate("Idle");
	    
	    dealWithRotations();
	}
	
	public IEnumerator freeze()
	{
	    float originalDashSpeed = dashSpeed;
        speed /= (global.difficultyFactor + 1);
        dashSpeed = speed / (global.difficultyFactor + 1);
	    Debug.Log(playerName + " is frozen!");
	    yield return new WaitForSeconds(1.5f * 0.8f * global.difficultyFactor);
        speed *= (global.difficultyFactor + 1);
	    yield return new WaitForSeconds(1.5f * 0.2f * global.difficultyFactor);
        dashSpeed = originalDashSpeed;
        yield break;
	}
	
	public IEnumerator paralyze()
	{
	    movementStyle originalMovementStyle = movements;
	    movements = movementStyle.Still;
	    Debug.Log(playerName + " is paralyzed!");
	    yield return new WaitForSeconds(1.25f * global.difficultyFactor);
        movements = originalMovementStyle;
        yield break;
	}
	
	public void OnCollisionEnter( Collision other ) 
    {
        if (!global.ongoingGame) return;
        
        if (other.collider.GetComponent<Rigidbody>() != null)
        {
            float magnitude = (other.relativeVelocity.magnitude * 0.5f) + 0.5f;
            AudioClip clip = (dashing) ? heavyHitSound : mediumHitSound;
            if (clip != null)
                global.playClipAt(clip, transform.position, magnitude);
        }
        if (other.collider.CompareTag("ground"))
        {
            groundless = false;
            body.drag = 5;
        }
        else if (other.collider.CompareTag("Player"))
        {
            if (other.collider.GetComponent<playerController>().isDashing)
                body.mass /= 2;
        }
    }
    
    public void OnTriggerEnter( Collider other )
    {
        if (!global.ongoingGame) return;
        
        if (other.CompareTag("fire")) 
        {
            burningUntil = global.now.AddSeconds(1.25f * global.difficultyFactor);
    	    Debug.Log(playerName + " is burning!");
        }
        else if (other.CompareTag("ice"))
        {
            StartCoroutine(freeze());
            global.currentArena.hideObject(other.gameObject);
        }
        else if (other.CompareTag("shock"))
        {
            StartCoroutine(paralyze());
            global.currentArena.hideObject(other.gameObject);
        }
    }
 
    public void OnCollisionExit( Collision other ) 
    {
        if (!global.ongoingGame) return;
        
        if (other.collider.CompareTag("ground"))
        {
            groundless = true;
            body.drag = 0;
        }
        else if (other.collider.CompareTag("Player"))
        {
            body.mass = mass;
        }
    }
}
