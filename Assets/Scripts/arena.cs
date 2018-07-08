using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arena : MonoBehaviour
{
    public enum arenaGroundShape : byte
    {
        Round = 1,
        Square = 2
    }
    
    public arenaGroundShape arenaShape;
    private Vector3 referencePoint, auxiliarPoint;
    private float arenaRadius, holeRadius;
    private float arenaSizeX, arenaSizeZ, holeSizeX, holeSizeZ;

    private System.DateTime lastPlayPauseTime;
    private bool paused;
    public bool isPaused { get { return paused; } }
	
	private bool isInsideRoundArenaArea( Vector3 position )
	{
	    float distance = Vector3.Distance(position, referencePoint);
	    if (distance > arenaRadius) return false;
	    if ((global.mode == global.arenaMode.Inverted) && (distance < holeRadius))
	        return false;
	    return true;
	}
	
	private bool isInsideSquareArenaArea( Vector3 position )
	{
	    if ((position.x < referencePoint.x) || (position.z < referencePoint.z) ||
	        (position.x > (referencePoint.x + arenaSizeX)) || 
	        (position.z > (referencePoint.z + arenaSizeZ)))
	        return false;
	    if (global.mode == global.arenaMode.Inverted)
	    {
	        if ((position.x > auxiliarPoint.x) && (position.x < (auxiliarPoint.x + holeSizeX)))
	            return false;
	        if ((position.z > auxiliarPoint.z) && (position.x < (auxiliarPoint.z + holeSizeZ)))
	            return false;
	    }
	    return true;
	}
	
	private void findReferencePoints()
	{
		if (arenaShape == arenaGroundShape.Square)
		{
		    GameObject A = global.getByName("ARENA_CORNER_A");
		    GameObject B = global.getByName("ARENA_CORNER_B");
		    referencePoint = A.transform.position;
		    arenaSizeX = B.transform.position.x - referencePoint.x;
		    arenaSizeZ = B.transform.position.z - referencePoint.z;
		}
		else
		{
		    GameObject A = global.getByName("ARENA_CENTER");
		    GameObject B = global.getByName("ARENA_BORDER");
		    referencePoint = A.transform.position;
		    arenaRadius = Vector3.Distance(referencePoint, B.transform.position);
		}
		referencePoint.y = global.getByName("arenaGround").transform.position.y;
	}
	
	private void findHoleReferencePoints()
	{
		if (arenaShape == arenaGroundShape.Square)
		{
		    GameObject A = global.getByName("ARENA_HOLE_CORNER_A");
		    GameObject B = global.getByName("ARENA_HOLE_CORNER_B");
		    auxiliarPoint = A.transform.position;
		    holeSizeX = B.transform.position.x - auxiliarPoint.x;
		    holeSizeZ = B.transform.position.z - auxiliarPoint.z;
		}
		else
		{
		    GameObject A = global.getByName("ARENA_HOLE_BORDER");
		    holeRadius = Vector3.Distance(referencePoint, A.transform.position);
		}
	}
	
	public bool isInsideArenaLimits( Vector3 position )
	{
	    if (position.y <= referencePoint.y) return false;
	    if (arenaShape == arenaGroundShape.Square) 
	        return isInsideSquareArenaArea(position);
	    else return isInsideRoundArenaArea(position);
	}
	
	private void preparePlayers()
	{
	    
	}
    
    public void setLightingColor( Color color )
    {
        GameObject[] lightSources = global.getByTag("mainLightSource");
        foreach (GameObject lightSource in lightSources)
            lightSource.GetComponent<Light>().color = color;
    }
	
	private void normalModeChanges()
	{
        setLightingColor(new Color(1.0F, 1.0F, 1.0F, 1.0F));
        global.setActiveByTag("fire", false);
        global.setActiveByTag("ice", false);
        global.setActiveByTag("shock", false);
	}
	
	private void invertedModeChanges()
	{
	    findHoleReferencePoints();
	}
	
	private void frozenModeChanges()
	{
        setLightingColor(new Color(0.72F, 0.92F, 1.0F, 1.0F));
        global.setRandomlyActiveByTag("ice");
	}
	
	private void burningModeChanges()
	{
        setLightingColor(new Color(1.0F, 0.89F, 0.7F, 1.0F));
        global.setRandomlyActiveByTag("fire");
	}
	
	private void electricModeChanges()
	{
        global.setRandomlyActiveByTag("shock");
	}
	
	private void shrunkenModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
	    foreach (GameObject x in players)
	    {
	        x.GetComponent<Transform>().localScale *= 0.5f;
	        x.GetComponent<Rigidbody>().mass *= 0.5f;
	    }
	}
	
	private void enlargedModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
	    foreach (GameObject x in players)
	    {
	        x.GetComponent<Transform>().localScale *= 2.0f;
	        x.GetComponent<Rigidbody>().mass *= 1.5f;
	    }
	}
	
	private void darkModeChanges()
	{
        setLightingColor(new Color(0.001F, 0.001F, 0.001F, 1.0F));
        global.getByName("darkModeFourthWall").SetActive(true);
	}
	
	private void ghostModeChanges()
	{
        Shader ghostShader = Shader.Find("Particles/Additive (Soft)");
	    foreach (GameObject x in global.getByTag("Player"))
	        x.GetComponent<Renderer>().material.shader = ghostShader;
	    foreach (GameObject x in global.getByTag("fakePlayer"))
	        x.GetComponent<Renderer>().material.shader = ghostShader;
        setLightingColor(new Color(0.05F, 0.05F, 0.05F, 1.0F));
	}
	
	private void setUpModeElements()
	{
		if (global.mode == global.arenaMode.Frozen)
		    frozenModeChanges();
		if (global.mode == global.arenaMode.Burning)
		    burningModeChanges();
		if (global.mode == global.arenaMode.Electric)
		    electricModeChanges();
		if (global.mode == global.arenaMode.Ghost)
		    ghostModeChanges();
		if (global.mode == global.arenaMode.Shrunken)
		    shrunkenModeChanges();
		if (global.mode == global.arenaMode.Enlarged)
		    enlargedModeChanges();
		if (global.mode == global.arenaMode.Dark)
		    darkModeChanges();
		else normalModeChanges();
	}
	
	public void Start()
	{
        global.currentArena = this;
	    paused = false;
        
	    GameObject[] music = global.getByTag("music");
	    if (music[0] != null) 
	        music[0].GetComponent<AudioSource>().volume = global.musicVolume;
		
		findReferencePoints();
		
		preparePlayers();
	        
	    global.ongoingGame = true;
	
		if (global.mode == global.arenaMode.Frozen)
		    frozenModeChanges();
		else if (global.mode == global.arenaMode.Burning)
		    burningModeChanges();
		else if (global.mode == global.arenaMode.Electric)
		    electricModeChanges();
		else if (global.mode == global.arenaMode.Ghost)
		    ghostModeChanges();
		else if (global.mode == global.arenaMode.Shrunken)
		    shrunkenModeChanges();
		else if (global.mode == global.arenaMode.Enlarged)
		    enlargedModeChanges();
		else if (global.mode == global.arenaMode.Dark)
		    darkModeChanges();
		else normalModeChanges();
	}
	
	public void PlayPause()
	{
        if (global.ongoingGame)
        {
            if (global.ongoingGame) global.ongoingGame = false;
            paused = true;
            global.getByName("pauseScreenBackground").SetActive(true);
	        Debug.Log("Paused");
	    }
	    else if (paused && (!global.ongoingGame))
	    {
            if (!global.ongoingGame) global.ongoingGame = true;
            paused = false;
            global.getByName("pauseScreenBackground").SetActive(false);
	        Debug.Log("Resumed");
	    }
	}
	
	public void FixedUpdate()
	{
	    if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Pause)) &&
	        ((global.now - lastPlayPauseTime).TotalMilliseconds > 200))
	    {
	        lastPlayPauseTime = global.now;
	        PlayPause();
	    }
	    else if ((!global.ongoingGame)) secretsHandler.readSecretCode();
	}
	
	public void finish( global.gameResult result, playerController winner = null )
	{
        global.ongoingGame = false;
	    if (result == global.gameResult.WIN)
	    {
	        if (global.clashMode)
	        {
	            global.clashVictories[winner.playerNumber - 1]++;
	            global.loadProperArenaScene();
	        }
	        else global.advanceCampaign();
	    }
	    else if (result == global.gameResult.DRAW)
	    {
            if (global.clashMode) global.loadProperArenaScene();
            else
            {
                if (global.bossEncounter) 
                {
                    global.bossEncounter = false;
                    // chama cutscene de derrota e volta pro menu principal
                }
                
                // uma cutscene para draw
            }
	    }
	    else global.restart(); // LOSE - só se aplica à campanha
	}
}
