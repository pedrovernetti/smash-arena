using System.Collections;
using UnityEngine;

public class programmedHinge : MonoBehaviour
{
    public char axis;
    public float maximumAngle;
    public float minimumAngle;
    public float speed;
    public float timeInterval;

    private float nextActionTime = 0.0f;
    private float currentAngle;
    private new Transform transform;
    
	public void Start() 
	{
		transform = GetComponent<Transform>();
		//currentAngle = 
	}
 
    public void Update()
    {
        if (Time.time > nextActionTime )
        {
            //currentAngle = 
            if (axis == 'Z') transform.Rotate(0.0f, 0.0f, speed);
            else if (axis == 'Y') transform.Rotate(0.0f, speed, 0.0f);
            else transform.Rotate(speed, 0.0f, 0.0f);
            nextActionTime += timeInterval;
        }
    }
}
