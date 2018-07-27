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
     
    public bool wantsToDash = false;
     
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
 	
 	private GameObject closestEnemy( bool forceChanging = false )
 	{
         GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
         GameObject closest = null;
         float distance = Mathf.Infinity, currentDistance;
         Vector3 difference;        
         
         foreach (GameObject player in players)
         {
             difference = player.transform.position - transform.position;
             currentDistance = difference.sqrMagnitude;
             if (forceChanging && (player == currentTarget)) continue;
             if (!(global.difficulty > global.difficultyLevel.Normal) ||
                 !(global.isMachineControlled(player)))
             {
                 if ((currentDistance > 0.0f) && (currentDistance < distance))
                 {
                     closest = player;
                     distance = currentDistance;
                 }
             }
         }
         
         return closest;
 	}
     
 	public void Start() 
 	{
 	    body = gameObject.GetComponent<Rigidbody>();
 	    controller = gameObject.GetComponent<playerController>();
 	    movements = controller.movements;
 	    
 	    setUpScanners();
 	    currentTarget = closestEnemy();
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
 	
 	private bool chase()
 	{
 	    if (currentTarget == null) return false;
 	    if (currentTarget.GetComponent<playerController>() == null) return false;
 	    if (currentTarget.GetComponent<playerController>().isGroundless) return false;
 	    if (movesLikeAChessPiece)
 	    {
 	        Vector3 nextPosition = 
 	            Vector3.MoveTowards(transform.position, currentTarget.transform.position, 1.0f);
 	        controller.control(nextPosition.x, nextPosition.z);
 	    }
 	    else
 	    {
            turnToTarget();
 	        controller.control(0.0f, 1.0f);
 	    }
        return true;
    }

    public void FixedUpdate()
    {
        if (controller.isGroundless) return;
        currentTarget = closestEnemy();
        if (!chase()) currentTarget = closestEnemy(true);

        //Debug.Log(gameObject.name + " is now chasing " + currentTarget.name);
    }
 }
