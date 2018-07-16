using System.Collections;
using UnityEngine;

public class easterEgg : MonoBehaviour
{
    public enum easterEggType : byte
    {
        Bridge = 1,
        Comet = 2,
        Cat = 4,
        Chess = 8
    }
    
    public easterEggType which;
    
	public void OnCollisionEnter( Collision other ) 
    {
        if (global.clashMode) return;
        else if ((which == easterEggType.Bridge) && (other.collider.CompareTag("deathPlane")))
        {
            Debug.Log("Bridge easter egg unlocked...");
        }
        else if ((which == easterEggType.Cat) && (other.collider.name == "cat"))
        {
            Debug.Log("Cat easter egg unlocked...");
        }
    }
}
