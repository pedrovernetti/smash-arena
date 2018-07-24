using System.Collections;
using UnityEngine;

public class easterEgg : MonoBehaviour
{
    public enum easterEggType : byte
    {
        Bridge = 1,
        Comet = 2,
        Cat = 3,
        Chess = 4,
        Mjolnir = 5
    }
    
    public easterEggType which;
    
	public void OnCollisionEnter( Collision other ) 
    {
        if (global.clashMode) return;
        else if ((which == easterEggType.Bridge) && (other.collider.CompareTag("Player")) &&
                 (GetComponent<Rigidbody>() != null))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else if ((which == easterEggType.Bridge) && (other.collider.CompareTag("deathPlane")))
        {
            Debug.Log("Bridge easter egg unlocked...");
        }
        else if ((which == easterEggType.Cat) && (other.collider.name == "cat"))
        {
            Debug.Log("Cat easter egg unlocked...");
        }
        else if ((which == easterEggType.Mjolnir ) && (GetComponent<Rigidbody>() != null))
        {
            if (other.collider.name.StartsWith("newton") ||
                other.collider.name.EndsWith("ewton"))
                GetComponent<Rigidbody>().mass = 1;
            else GetComponent<Rigidbody>().mass = 1000;
        }
    }
}
