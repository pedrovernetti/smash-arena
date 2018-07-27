using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[DisallowMultipleComponent]
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
    private Image portrait;
    private Text portraitText;
    
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
	private AI intelligence;
	public bool hasIntelligence { get { return (intelligence != null); } }
	private bool groundless;
	private bool dashing;
    public bool isDashing { get { return dashing; } }    
    public bool isMoving { get { return (body.velocity.magnitude > 0.1); } }
    private System.DateTime burningUntil;
    public bool isBurning { get { return (global.now <= burningUntil); } }
    private bool ready = false;
    public bool isReady { get { return ready; } }
    
    private string lastAnimationBool;
    
    public bool isGroundless
    { 
        get { return groundless; }
        set { groundless = value; }
    }
    
    public bool isTerminallyGroundless
    {
        get { return (isGroundless && (!isMoving)); }
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
	
	public void Awake()
	{
        gameObject.SetActive(false);
            
	    for (int i = 0; i < 4; i++)
        {
            if (global.playerTypes[i] == global.playerType.Disabled) continue;
            if (global.playerCharacters[i] == gameObject.name)
            {
                playerType = global.playerTypes[i];
                playerNumber = i + 1;
                ABCD = (char)('@'+playerNumber);
                gameObject.SetActive(true);
            }
        }
	}
	
	private void placeProperly()
	{
	    GameObject reference = global.getByName("PLAYER" + (char)('0'+playerNumber) + "_PLACE");
	    if (reference != null) transform.position = reference.transform.position;
	    else transform.position = global.currentArena.randomArenaPosition(transform.position.y);
	}
    
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
    
    private void findPortraitAndSignIt()
    {
        GameObject UIElement = global.getByName("player" + playerNumber + "Name");
	    if ((UIElement != null) && (UIElement.GetComponent<Text>() != null))
	    {
	        portraitText = UIElement.GetComponent<Text>();
	        portraitText.text = playerName;	   
	    }
	    else portraitText = null;
        
	    UIElement = global.getByName("player" + playerNumber + "Portrait");
	    portrait = null;
        if (UIElement != null)
        {
            Transform[] all = UIElement.GetComponentsInChildren<Transform>(true);
            foreach (Transform transform in all)
            {
                if (transform.gameObject.activeInHierarchy && 
                    (transform.gameObject.GetComponent<Image>() != null)) 
                    portrait = transform.gameObject.GetComponent<Image>();
            }
        }
    }
    
	public void Start() 
	{
        placeProperly();
	    
	    setUpRigidbody();
	    defaultRotation = transform.rotation;
		groundless = false;
        
        animator = GetComponent<Animator>();
        hasAnimator = (animator != null) ? true : false;
	    
	    if (playerType == global.playerType.Machine)
	        intelligence = gameObject.AddComponent<AI>();
	    else 
	    {
	        intelligence = null;
	        if (playerType == global.playerType.BrainDead)
	            movements = movementStyle.Still;
	    }
	        
	    burningUntil = global.now.AddSeconds(-1);
	    
	    Debug.Log(playerName + ": " + playerType + ", " + (GetComponent<AI>() != null));
	         
	    findPortraitAndSignIt();
	    
	    ready = true;
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
	
	private bool isTryingToDash()
	{
        if (Input.GetButton("dash" + ABCD)) return true;
        else if (hasIntelligence && intelligence.wantsToDash) return true;
	    else return false;
	}
	
	private void carControl( float horizontal, float vertical )
	{
	    if (vertical < 0.0f) horizontal *= -1.0f;
	    if (isMoving) transform.Rotate(Vector3.up, horizontal * speed * Time.deltaTime);
        if (groundless) return;
        if (vertical != 0.0f) vertical = (vertical > 0.0f) ? 0.25f : -0.25f;
        body.velocity = transform.forward * speed * vertical;
		if (isTryingToDash() && isMoving && (vertical > 0.0f))
		{
		    body.velocity = transform.forward * dashSpeed * vertical;
		    dashing = true;
		}
		else dashing = false;
	}
	
	private void creatureControl( float horizontal, float vertical )
	{
	    if (vertical < 0.0f) horizontal *= -1.0f;
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
		if (isTryingToDash() && isMoving && (vertical > 0.0f))
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
        if (isTryingToDash() && isMoving)
    	{
    	    body.AddForce(movement * dashSpeed);
    		dashing = true;
    	}
    	else dashing = false;
	}
	
	
	
	public void control( float horizontal = 0.0f, float vertical = 0.0f )
	{
	    float h = global.noiseFreeValue(((horizontal != 0.0f) || isMachine) ? 
	                horizontal : global.horizontalInput(ABCD));
	    float v = global.noiseFreeValue(((vertical != 0.0f) || isMachine) ? 
	                vertical : global.verticalInput(ABCD));
	    
	    if ((h == 0.0f) && (v == 0.0f)) return;
	    else if (movements == movementStyle.Creature) creatureControl(h, v);
	    else if (movements == movementStyle.ChessPiece) chessControl(h, v);
	    else if (movements == movementStyle.Car) carControl(h, v);
	}
	
	private void dealWithRotations()
	{
        if ((!global.currentArena.isInsideArenaLimits(transform.position)) &&
            (movements != movementStyle.Creature))
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
        if ((!global.ongoingGame) || (!isReady)) return;
        
	    if (groundless && !global.currentArena.isInsideArenaLimits(transform.position))
	        body.AddForce(Physics.gravity * body.mass * 2);
	    else if (isBurning && global.chance(33.0f))
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
            Debug.Log("collides with ground");
            groundless = false;
            body.mass = mass;
            body.drag = 5;
        }
        else if (other.collider.CompareTag("Player"))
        {
            if (other.collider.GetComponent<playerController>().isDashing)
                body.mass /= 2;
        }
        else Debug.Log("collides with " + other.collider.name);
    }
    
    public void die()
    {
        Debug.Log(playerName + " died");
        gameObject.SetActive(false);
        global.currentArena.setDead(this);
        if (hasIntelligence) Object.Destroy(GetComponent<AI>());
        if (portrait != null) 
            portrait.color = global.changedTransparency(portrait.color, 0.33f);
        if (portraitText != null) 
            portraitText.color = global.changedTransparency(portraitText.color, 0.33f);
    }
    
    public void OnTriggerEnter( Collider other )
    {
        
        if (other.CompareTag("deathPlane")) die();
        else if (!global.ongoingGame) return;
        else if (other.CompareTag("fire")) 
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
            Debug.Log("collides out with ground");
            groundless = true;
            if (!global.currentArena.isInsideArenaLimits(transform.position))
                body.mass *= 1000;
            body.drag = 0;
        }
        else if (other.collider.CompareTag("Player"))
        {
            body.mass = mass;
        }
    }
}
