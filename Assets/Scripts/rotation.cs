using System.Collections;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public float xRate;
    public float yRate;
    public float zRate;

    private new Transform transform;
    
	public void Start() 
	{
		transform = GetComponent<Transform>();
	}
    
    public void FixedUpdate()
    {
        transform.Rotate(xRate, yRate, zRate);
    }
}
