using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class collisionScanner : MonoBehaviour
{
    private ArrayList collidingObjectsList;
    
    public ArrayList collidingObjects { get { return collidingObjectsList; } }

	public void OnCollisionEnter( Collision other ) 
    {
        if (other.gameObject.layer != 8)
            collidingObjects.Add(other.gameObject);
    }
    
	public void OnCollisionExit( Collision other ) 
    {
        if (other.gameObject.layer != 8)
            collidingObjects.Remove(other.gameObject);
    }
}
