using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class collisionScanner : MonoBehaviour
{
    private ArrayList collidingObjectsList;
    
    public ArrayList collidingObjects { get { return collidingObjectsList; } }
    
    public void Start()
    {
        collidingObjectsList = new ArrayList();
    }

    public void OnTriggerEnter( Collider other )
    {
        Debug.Log("scanned: " + other.gameObject.name);
        if (other.gameObject.layer != 8)
            collidingObjectsList.Add(other.gameObject);
        Debug.Log("ending scanning");
    }
    
    public void OnTriggerExit( Collider other )
    {
        if (other.gameObject.layer != 8)
            collidingObjectsList.Remove(other.gameObject);
    }
}
