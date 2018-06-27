using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // player's data
    public string playerName;
    public int playerNumber;
    private char ABCD;
    
    // control
    private Rigidbody body;
    private new Transform transform;
	private float originalMass;
	public float speed = 80;
	public float dashSpeed = 200;
	public bool chessStyleControls = false;
	private bool groundless; // está "sem chão" ou não
	private bool dashing;
	
    public bool isDashing { get { return dashing; } }    
    public bool isMoving { get { return (body.velocity.magnitude > 0.01); } }
	
	// sounds
	public AudioClip mediumHit;
	public AudioClip heavyHit;
    
    public bool isCampaignPlayer
    { 
        get { return ((!global.clashMode) && (playerNumber == 1)); }
    }    
    
    public bool isActivePlayer
    { 
        get { return (playerNumber <= global.playersCount); }
    }
    
    public bool isGroundless
    { 
        get { return groundless; }
        set { groundless = value; }
    }
    
	public void Start() 
	{
	    if ((playerNumber > global.playersCount) || (playerNumber < 1) ||
	        (global.bossEncounter && !isCampaignPlayer && (name != "boss")) ||
	        (!global.bossEncounter && (name == "boss")))
	    {
	        gameObject.SetActiveRecursively(false);
	        return;
	    }
	    
	    if (global.clashMode) playerName = global.playerNames[playerNumber - 1];
	    if (!((playerNumber > 0) && (playerNumber < 5))) ABCD = 'A';
	    else ABCD = (char)('@'+playerNumber);
	    
		body = GetComponent<Rigidbody>();
		transform = GetComponent<Transform>();
        originalMass = body.mass;		
		groundless = false;
        body.constraints = RigidbodyConstraints.FreezeRotationX | 
                           RigidbodyConstraints.FreezeRotationZ;
	}
	
    public void FixedUpdate()
    {
        if (global.ongoingGame)
        {
		    float moveHorizontal = Input.GetAxis("horizontal" + ABCD);
		    float moveVertical = Input.GetAxis("vertical" + ABCD);
		    Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		    if (body.drag <= 0) body.AddForce(Physics.gravity * body.mass * 2);
		
		    transform.Rotate(Vector3.up, moveVertical * speed * Time.deltaTime);
            if ((!groundless) && (global.ongoingGame))
            {
		        body.AddForce (movement * speed);
        		if (Input.GetButton("dash" + ABCD) && isMoving)
        		{
        		    body.AddForce(movement * dashSpeed);
        		    dashing = true;
        		}
        		else dashing = false;
        	}
        }
	} 
	
	public IEnumerator freeze( float factor, float duration )
	{
	    float originalDashSpeed = dashSpeed;
        speed /= factor;
        dashSpeed = 1;
        yield return new WaitForSeconds(duration * 0.8f);
        speed *= factor;
        yield return new WaitForSeconds(duration * 0.2f);
        dashSpeed = originalDashSpeed;
	}
	
	public IEnumerator burn( float factor, float duration )
	{
        yield return new WaitForSeconds(duration);
	}
	
	public IEnumerator paralyze( float factor, float duration )
	{
        yield return new WaitForSeconds(duration);
	}
	
	public void OnCollisionEnter( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = false;
            body.drag = 5;
        }
        else if (other.collider.CompareTag("Player"))
        {
            if (other.collider.GetComponent<playerController>().isDashing)
                body.mass /= 2;
                
            float magnitude = (other.relativeVelocity.magnitude * 0.5f) + 0.5f;
            AudioClip clip = (dashing) ? heavyHit : mediumHit;
            if (clip != null)
                global.playClipAt(clip, transform.position, magnitude);
        }
        else if (other.collider.CompareTag("ice"))
        {
            StartCoroutine(freeze(3f, 4f));
        }
        else if (other.collider.CompareTag("fire"))
        {
            StartCoroutine(burn(3f, 4f));
        }
        else if (other.collider.CompareTag("shock"))
        {
            StartCoroutine(paralyze(3f, 4f));
        }
    }
 
    public void OnCollisionExit ( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = true;
            body.drag = 0;
        }
        else if (other.collider.CompareTag("Player"))
        {
            body.mass = originalMass;
        }
    }
}
