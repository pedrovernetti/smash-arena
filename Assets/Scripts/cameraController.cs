using System.Collections;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    private float max;
    private float min;
    
    public bool zoomingIn
    { 
        get { return (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals)); }
    }
    
    public bool zoomingOut
    { 
        get { return (Input.GetKey(KeyCode.Minus)); }
    }
    
	public void Start() 
	{
		max = transform.position.z + 7.0f;
		min = transform.position.z - 12.0f;
		
	    if (global.mode == global.arenaMode.DownsideUp)
	        transform.Rotate(Vector3.forward * 180);
	}
    
    public void FixedUpdate()
    {
	    if (zoomingIn && (transform.position.z <= max))
	    {
	        transform.Translate(0.0f, -0.1f, 0.2f);
	        transform.Rotate(-0.075f, 0.0f, 0.0f);
	    }
	    if (zoomingOut && (transform.position.z >= min))
	    {
	        transform.Translate(0.0f, 0.1f, -0.2f);
	        transform.Rotate(0.075f, 0.0f, 0.0f);
	    }
    }
}
