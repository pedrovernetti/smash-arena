using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
    public Text deathText;

    private Rigidbody rb;
	private int count;
	
	private AudioClip knock_medium;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		count = 0;
		
	}
    void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal,0.0f, moveVertical);
		    rb.AddForce (movement * speed);
		if (Input.GetButton ("Fire1")){
			rb.AddForce (movement*10);

		}
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deathPlane"))
        {
            rb.gameObject.SetActive(false);
            if(deathText.text=="")
                deathText.text = "Black Knight WIN";
        }
    }
    void OnCollisionEnter ()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = knock_medium;
        audio.Play();
    }
}
