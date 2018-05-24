using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public string playerName;
    public int playerNumber;
    
	public float speed;
	public bool groundless; // está "sem chão" ou não

    private Rigidbody rb;

	void Start () 
	{
	    if (global.clashMode) playerName = global.playerNames[2];
		rb = GetComponent<Rigidbody> ();
		groundless = false;
	}
	
    void FixedUpdate ()
    {
		float moveHorizontal = Input.GetAxis ("horizontal" + (char)('@'+playerNumber) );
		float moveVertical = Input.GetAxis ("vertical" + (char)('@'+playerNumber) );
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		if (rb.drag <= 0) rb.AddForce(Physics.gravity * rb.mass * 2);
		
        if (!groundless) // só controla se não estiver sem chão
        {
		    rb.AddForce (movement * speed);
    		if (Input.GetButton("Fire1")) rb.AddForce(movement * 100);
    	}
	} 
	
	void OnCollisionEnter( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = false;
            rb.drag = 5;
        }
        else if (other.collider.CompareTag("Player"))
        {
            AudioSource knock = GetComponents<AudioSource>()[0]; // TODO
            knock.Play();
        }
    }
 
    void OnCollisionExit ( Collision other ) 
    {
        if (other.collider.CompareTag("ground"))
        {
            groundless = true;
            rb.drag = 0;
        }
    }
}
