using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;
	private int count;

	void Start () {
		
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		if (EnergyBarScript.energy < 100)
			EnergyBarScript.energy++;

		rb.AddForce (movement * speed);

		if (Input.GetButton ("Fire1") && (EnergyBarScript.energy == 100)) {
			rb.AddForce (rb.velocity.normalized * 10, ForceMode.Impulse);
			EnergyBarScript.energy = 0;

		}
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("PickUp")) {
			other.gameObject.SetActive(false);
			count++;
			SetCountText ();
		}
	}

	void SetCountText () {
		countText.text = "Count: " + count.ToString ();
		if (count >= 8)
			winText.text = "You Win!";
	}

}