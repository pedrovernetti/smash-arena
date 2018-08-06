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
        if ((other.gameObject.layer != 8) && (!other.gameObject.CompareTag("ground")))
            collidingObjectsList.Add(other);
    }
    
    public void OnTriggerExit( Collider other )
    {
        if (other.gameObject.layer != 8)
            collidingObjectsList.Remove(other);
    }
}
