﻿using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class orbit : MonoBehaviour
{
    private Vector3 direction;
    public float speed;
    public float deviationFromSkyline;
    public bool clockwise;
    public GameObject center;
    
	public void Start() 
	{
	    direction = new Vector3(deviationFromSkyline, 0.0f, 0.0f);
	    direction += (clockwise) ? Vector3.up : Vector3.down;
	}
    
    public void FixedUpdate()
    {
        if (global.ongoingGame)
        {
            transform.RotateAround (center.transform.position, 
                                    direction, 
                                    (speed * Time.deltaTime));
        }
    }
}
