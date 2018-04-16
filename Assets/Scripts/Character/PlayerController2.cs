using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

	public float speed;
	public Text countText;

	private Rigidbody rb;
	private int count;
	Vector3 movement;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		movement = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Fire1");
		float moveVertical = Input.GetAxis ("Fire1");

		movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

	}
		
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			countText.text = "Debug: " + contact.normal.ToString ();

		}

	}

}