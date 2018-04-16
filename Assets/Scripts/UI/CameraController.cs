using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject knight;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - knight.transform.position;
	}

	// Update is called once per frame
	void LateUpdate () {
		transform.position = knight.transform.position + offset;
	}
}
