using System.Collections;
using UnityEngine;

public class ground : MonoBehaviour
{
    private bool falling;
    private new Transform transform;
    private Rigidbody body;
    
	public void Start() 
	{
		falling = false;
		body = GetComponent<Rigidbody>();
		transform = GetComponent<Transform>();
		
	    if (global.mode != global.arenaMode.Unstable)
	    {
	        body.isKinematic = true;
	        
	        if (global.mode == global.arenaMode.Ghost)
	        {
	            if (global.theme == global.arenaTheme.Chess)
    	            GetComponent<MeshRenderer>().materials[0] = 
    	                GetComponent<MeshRenderer>().materials[2];
	        }
	    }
	    else 
	    {
	        body.isKinematic = false;
	        if (global.mode == global.arenaMode.Inverted)
	        {
	             if (name == "arenaGround") 
	                gameObject.SetActiveRecursively(false);
	        }
	        else if (name == "invertedArenaGround") 
	            gameObject.SetActiveRecursively(false);
	    }
	}
	
	private void fall()
	{
        falling = true;
	    GameObject[] players = global.getByTag("Player");
	    foreach (GameObject player in players)
	        player.GetComponent<playerController>().isGroundless = true;
        gameObject.tag = "Untagged";
	}
	
    public void FixedUpdate()
    {
        if ((Mathf.Abs(transform.rotation.x) > 40) || 
            (Mathf.Abs(transform.rotation.y) > 40))
            fall();
	    if (falling) body.AddForce(Physics.gravity * body.mass * 5);
	} 
 
    public void OnCollisionExit( Collision other ) 
    {
        if (other.collider.CompareTag("arenaBase")) fall();
    }
}
