using System.Collections;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public bool fixedZoom;
    private float max;
    private float min;

    private new Transform transform;
    private Rigidbody body;
    private Skybox skybox;
    
	public void Start() 
	{
		transform = GetComponent<Transform>();
		max = transform.position.z + 7.0f;
		min = transform.position.z - 12.0f;
		body = GetComponent<Rigidbody>();
		skybox = GetComponent<Skybox>();
		
	    if ((global.difficulty == global.difficultyLevel.Hell) &&
	        (global.mode == global.arenaMode.Inverted) &&
	        (global.theme == global.arenaTheme.Chess))
	        transform.Rotate(Vector3.forward * 180);
	}
    
    public void FixedUpdate()
    {
		    float move = Input.GetAxis("zoom");
		    if ((!fixedZoom) && (move > 0) && (transform.position.z <= max))
		    {
		        transform.Translate(0.0f, -0.1f, 0.2f);
		        transform.Rotate(-0.075f, 0.0f, 0.0f);
		    }
		    if ((!fixedZoom) && (move < 0) && (transform.position.z >= min))
		    {
		        transform.Translate(0.0f, 0.1f, -0.2f);
		        transform.Rotate(0.075f, 0.0f, 0.0f);
		    }
    }
}
