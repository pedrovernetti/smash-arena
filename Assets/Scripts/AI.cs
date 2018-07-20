using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    private static float LOSMultiplier = ((int)(global.difficulty) / 2);
    private static Vector3[] scannersDefaultPosition = 
        new Vector3[] 
        {
            new Vector3(2.0f, 0.0f, 0.0f) * LOSMultiplier,
            new Vector3(1.5f, 0.0f, 1.5f) * LOSMultiplier,
            new Vector3(0.0f, 0.0f, 2.0f) * LOSMultiplier,
            new Vector3(-1.5f, 0.0f, 1.5f) * LOSMultiplier,
            new Vector3(-2.0f, 0.0f, 0.0f) * LOSMultiplier,
            new Vector3(-1.5f, 0.0f, -1.5f) * LOSMultiplier,
            new Vector3(0.0f, 0.0f, -2.0f) * LOSMultiplier,
            new Vector3(1.5f, 0.0f, -1.5f) * LOSMultiplier
        };
    
    private Rigidbody body;    
    private playerController controller; 
    private playerController.movementStyle movements;
    private collisionScanner[] scanners;
    private GameObject currentTarget;
    private System.DateTime whenToReturnToAction;
    
    private bool isMoving
    { 
        get { return (body.velocity.magnitude > 0.1); }
    }
	private bool movesLikeACreature
	{
    	get { return (movements == playerController.movementStyle.Creature); }
	}	
	private bool movesLikeAChessPiece
	{
	    get { return (movements == playerController.movementStyle.ChessPiece); }
	}	
	private bool movesLikeACar
	{
	    get { return (movements == playerController.movementStyle.Car); }
	}
    
    private void setUpScanners()
    {
        GameObject[] scannerObjects = new GameObject[8];
        SphereCollider collider;
        scanners = new collisionScanner[8];
        for (int i = 0; i < 8; i++)
        {
            scannerObjects[i] = 
                new GameObject(gameObject.name + "Scanner" + i.ToString());
            scannerObjects[i].transform.parent = gameObject.transform;
            scannerObjects[i].transform.localPosition = scannersDefaultPosition[i];
            scannerObjects[i].layer = 8;
            
            collider = 
                scannerObjects[i].AddComponent<SphereCollider>() as SphereCollider;
            collider.radius = 1.0f;
            collider.isTrigger = true;
            
            scanners[i] =
                scannerObjects[i].AddComponent<collisionScanner>() as collisionScanner;
        }
    }
	
	private GameObject closestEnemy( bool forceAny = false )
	{
	    if (whenToReturnToAction > global.now) return null;
	    
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity, currentDistance;
        Vector3 difference;
        
        foreach (GameObject player in players)
        {
            difference = player.transform.position - transform.position;
            currentDistance = difference.sqrMagnitude;
            if ((global.difficulty > global.difficultyLevel.Normal) &&
                (global.isMachineControlled(player)) && !forceAny &&
                global.chance(50.0f / global.difficultyFactor))
                continue;
            if ((currentDistance > 0.0f) && 
                (currentDistance < (distance + Random.Range(0.0f, 4.0f))))
            {
                closest = player;
                distance = currentDistance;
            }
        }
        
        return ((closest != null) ? closest : closestEnemy(true));
	}
    
	public void Start() 
	{
	    body = gameObject.GetComponent<Rigidbody>();
	    controller = gameObject.GetComponent<playerController>();
	    movements = controller.movements;	    
	    setUpScanners();
	    Debug.Log(controller.playerName + " is machine-controlled");
	}
	
	private void turnToTarget()
	{
	    if (((movesLikeACar) && (isMoving)) || (!movesLikeACar))
	    {
            float rate = 1f;
            
            Transform target = currentTarget.transform;
            Quaternion targetRotation = 
                Quaternion.LookRotation(target.position - transform.position);
            rate = Mathf.Min(rate * Time.deltaTime, 1);
            transform.rotation = 
                Quaternion.Lerp(transform.rotation, targetRotation, rate);
        }
	}
	
	private void think()
	{
	    whenToReturnToAction = 
	        global.now.AddSeconds(Random.Range(1.0f, 6.0f / global.difficultyFactor));
	    currentTarget = null;
	}
	
	private void chase()
	{
	    if (movesLikeAChessPiece)
	    {
	        Vector3 nextPosition = 
	            Vector3.MoveTowards(transform.position, currentTarget.transform.position, 1.0f);
	        controller.control(nextPosition.x * -1, nextPosition.z * -1);
	    }
	    else
	    {
            turnToTarget();
	        controller.control(0.0f, 1.0f);
	    }
	}
    
    public void FixedUpdate()
    {
        if (!global.ongoingGame) return;
        
        currentTarget = closestEnemy();
        
        if (global.chance(35.0f / global.difficultyFactor)) 
            think();
        
        if (!global.chance(10.0f / global.difficultyFactor) && (currentTarget != null) &&
            !(currentTarget.GetComponent<playerController>() != null) &&
            !(currentTarget.GetComponent<playerController>().isGroundless))
            chase();
        
        if (currentTarget != null) Debug.Log(gameObject.name + " is now chasing " + currentTarget.name);
    }
}
