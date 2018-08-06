using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testSubject : MonoBehaviour
{
    private arena arena;
    private Rigidbody body;
    private Vector3 movement;
    private Transform[] children;
    
    public void Start()
    {
        arena = global.getByName("arena").GetComponent<arena>();
        body = GetComponent<Rigidbody>();
        children = gameObject.GetComponentsInChildren<Transform>(true);
    }
    
    public void FixedUpdate()
    {
        if (!global.ongoingGame)
        {
            movement = new Vector3 (Input.GetAxis("horizontalA"), Input.GetAxis("verticalB"), Input.GetAxis("verticalA"));
            transform.localScale = 
                new Vector3(transform.localScale.x + (Input.GetAxis("verticalC") / 10), 
                            transform.localScale.y + (Input.GetAxis("verticalC") / 10),
                            transform.localScale.z + (Input.GetAxis("verticalC") / 10));
            body.mass += (Input.GetAxis("verticalC") / 10);
            body.AddForce(movement * 30);
            Debug.Log("subject is on arena: " + arena.isInsideArenaLimits(transform.position).ToString());
            Debug.Log("subject position: X:" + transform.position.x + " Y:" + transform.position.y + " Z:" + transform.position.z);
            Debug.Log("subject scale: X:" + transform.localScale.x + " Y:" + transform.localScale.y + " Z:" + transform.localScale.z);
        }
        
        foreach (Transform child in children)
        {
            if (!arena.isInsideArenaLimits(child.position))
                Debug.Log(child.name + " outside arena");
        }
    }
}
