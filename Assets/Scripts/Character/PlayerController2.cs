using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

	public float speed;
	public Text deathText;

	private Rigidbody rb;
	private int count;
	Vector3 movement;
	
	AudioClip knock_medium;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		movement = new Vector3 (0.0f, 0.0f, 0.0f);
	}

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("horizontal2");
        float moveVertical = Input.GetAxis("vertical2");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        if (Input.GetButton("Fire3"))
        {
            rb.AddForce(movement * 100);

        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deathPlane"))
        {
            rb.gameObject.SetActive(false);
            if (deathText.text == "")
            deathText.text = "White Knight WIN";
        }
    }
    void OnCollisionEnter ()
    {
        knock_medium.LoadAudioData();
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = knock_medium;
        audio.Play();
    }
}
