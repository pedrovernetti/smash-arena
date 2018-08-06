using System.Collections;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public float xRate;
    public float yRate;
    public float zRate;
    
    public void FixedUpdate()
    {
        if(global.ongoingGame) transform.Rotate(xRate, yRate, zRate);
    }
}
