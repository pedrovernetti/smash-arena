using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public string playerName;
    public int playerNumber;
    private char ABCD;
    
	public float speed;
	public float dashSpeed;
	public bool chessStyleControls;
	private bool groundless; // está "sem chão" ou não
	
	public AudioClip mediumHit;
	public AudioClip heavyHit;

    private Rigidbody body;
    private Transform transform;
    
    public bool isGroundless
    { 
        get { return groundless; }
    }

	void Start () 
	{
	    if (global.clashMode) playerName = global.playerNames[playerNumber - 1];
	    if (!((playerNumber > 0) && (playerNumber < 5))) ABCD = 'A';
	    else ABCD = (char)('@'+playerNumber);
	        
		groundless = false;
		
		body = GetComponent<Rigidbody>();
		transform = GetComponent<Transform>();
	}
	
    void FixedUpdate ()
    {
        if (global.ongoingGame)
        {
		    float moveHorizontal = Input.GetAxis("horizontal" + ABCD);
		    float moveVertical = Input.GetAxis("vertical" + ABCD);
		    Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		    if (body.drag <= 0) body.AddForce(Physics.gravity * body.mass * 2);
		
		    transform.Rotate(Vector3.up, moveVertical * speed * Time.deltaTime);
            if (!groundless) // só controla se não estiver sem chão
            {
		        body.AddForce (movement * speed);
        		if (Input.GetButton("dash" + ABCD)) 
        		    body.AddForce(movement * dashSpeed);
        	}
        }
	} 
	
	void OnCollisionEnter( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = false;
            body.drag = 5;
        }
        else if (other.collider.CompareTag("Player"))
        {
            float magnitude = 
                    other.relativeVelocity.magnitude * body.velocity.magnitude;
            AudioClip clip = 
                    (Input.GetButton("dash" + ABCD)) ? heavyHit : mediumHit;
            if (clip != null)
                global.playClipAt(clip, transform.position, magnitude);
        }
    }
 
    void OnCollisionExit ( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = true;
            body.drag = 0;
        }
    }
}
