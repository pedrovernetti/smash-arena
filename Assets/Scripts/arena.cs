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
    
    private ArrayList modeObjects;
    private bool modeHasObjects;
    private Vector3 objectsRestingPlace;
    private bool modeObjectsAreResting;
    private float maximumObjectsSwitchInterval;
    private System.DateTime nextModeObjectsSwitch;

    private System.DateTime lastPlayPauseTime;
    private bool paused;
    public bool isPaused { get { return paused; } }
    
    #if UNITY_EDITOR
    private void setThemeBasedOnScene()
    {
        if (global.currentScene == "cars") 
            global.theme = global.arenaTheme.Cars;
        else if (global.currentScene == "humanoids")
            global.theme = global.arenaTheme.Humanoids;
        else if (global.currentScene == "fantasy")
            global.theme = global.arenaTheme.Fantasy;
        else if (global.currentScene == "chess")
            global.theme = global.arenaTheme.Chess;
    }
    #endif
	
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
	
	public bool isInsideArenaLimits( Vector3 position )
	{
	    if (position.y <= referencePoint.y) return false;
	    if (arenaShape == arenaGroundShape.Square) 
	        return isInsideSquareArenaArea(position);
	    else return isInsideRoundArenaArea(position);
	}
	
	public Vector3 randomArenaPosition( float y )
	{
	    Vector3 position;
		if (arenaShape == arenaGroundShape.Square)
		{
    	    float x, z;
		    x = Random.Range(referencePoint.x, (referencePoint.x + arenaSizeX));
		    z = Random.Range(referencePoint.z, (referencePoint.z + arenaSizeZ));
		    position = new Vector3(x, y, z);
		}
		else
		{
		    position = referencePoint;
		    position += (Vector3)(Random.insideUnitCircle * arenaRadius);
		    position.y = y;
		}
		return ((isInsideArenaLimits(position)) ? position : randomArenaPosition(y));
	}
	
	private void preparePlayers()
	{
	    /*string[] players = new string[4];
	    if (!global.clashMode)
	    {
	        players[0] = global.mainCharacter();
	        for (int i = 1; i < 4; i++) 
	            players[i] = global.randomCharacter();
	    }
	    else
	    {
	        for (int i = 0; i < 4; i++)     
	            players[i] = global.clashPlayerCharacters[i];
	    }
	    
	    GameObject[] characters = global.getByTag("Player");
	    for (int i = characters.Length - 1, j = 0, ok = 0; i >= 0; i--, ok = 0)
	    {
	        for (j = 0; j < 4; j++)
	        {
	            if (characters[i].name == players[j])
	            {
	                characters[i].GetComponent<playerController>().playerNumber = 
	                    j + 1;
	                characters[i].transform.position = 
	                    randomArenaPosition(characters[i].transform.position.y);
	                ok = 1;
	            }
	        }
	        if (ok == 0) Object.Destroy(characters[i]);
	    }*/
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
        modeHasObjects = false;
	}
	
	private void invertedModeChanges()
	{
	    findHoleReferencePoints();
        modeHasObjects = false;
	}
	
	private void frozenModeChanges()
	{
        setLightingColor(new Color(0.72F, 0.92F, 1.0F, 1.0F));
        global.setActiveByTag("ice");
        global.setActiveByTag("fire", false);
        global.setActiveByTag("shock", false);
        modeObjects = new ArrayList(global.getByTag("ice"));
        modeHasObjects = true;
	}
	
	private void burningModeChanges()
	{
        setLightingColor(new Color(1.0F, 0.89F, 0.7F, 1.0F));
        global.setActiveByTag("ice", false);
        global.setActiveByTag("fire");
        global.setActiveByTag("shock", false);
        modeObjects = new ArrayList(global.getByTag("fire"));
        modeHasObjects = true;
	}
	
	private void electricModeChanges()
	{
        global.setActiveByTag("ice", false);
        global.setActiveByTag("fire", false);
        global.setActiveByTag("shock");
        modeObjects = new ArrayList(global.getByTag("shock"));
        modeHasObjects = true;
	}
	
	private void shrunkenModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
	    foreach (GameObject x in players)
	    {
	        x.GetComponent<Transform>().localScale *= 0.5f;
	        x.GetComponent<Rigidbody>().mass *= 0.5f;
	    }
        modeHasObjects = false;
	}
	
	private void enlargedModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
	    foreach (GameObject x in players)
	    {
	        x.GetComponent<Transform>().localScale *= 2.0f;
	        x.GetComponent<Rigidbody>().mass *= 1.5f;
	    }
        modeHasObjects = false;
	}
	
	private void darkModeChanges()
	{
        setLightingColor(new Color(0.001F, 0.001F, 0.001F, 1.0F));
        global.getByName("darkModeFourthWall").SetActive(true);
        modeHasObjects = false;
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
		if (global.mode == global.arenaMode.Inverted)
		    frozenModeChanges();
		else if (global.mode == global.arenaMode.Frozen)
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
	
	private void setMusicVolume()
	{
	    GameObject[] music = global.getByTag("music");
	    if (music[0] != null) 
	        music[0].GetComponent<AudioSource>().volume = global.musicVolume;
	}
	
	public void modeObjectsSwitch()
	{
	    Debug.Log("[mode objects switch]");
	    nextModeObjectsSwitch =
	        global.now.AddSeconds(Random.Range(2.0f, maximumObjectsSwitchInterval));
	    if (modeObjectsAreResting)
	    {
	        modeObjectsAreResting = false;
	        if (global.ongoingGame) foreach (GameObject modeObject in modeObjects)
	        {
	            modeObject.transform.position = 
	                randomArenaPosition(modeObject.transform.position.y);
	        }
	    }
	    else
	    {
	        modeObjectsAreResting = true;
	        if (global.ongoingGame) foreach (GameObject modeObject in modeObjects)
	        {
	            modeObject.transform.position = objectsRestingPlace;
	        }
	    }
	}

	private void startModeObjectsCycle()
	{	
		if (global.difficulty < global.difficultyLevel.Hard)
		{
		    Object.Destroy((GameObject)(modeObjects[3]));
		    modeObjects.RemoveAt(3);
		    Object.Destroy((GameObject)(modeObjects[2]));
		    modeObjects.RemoveAt(2);
		}
		
    	objectsRestingPlace = 
    	    global.getByName("OBJECTS_RESTING_PLACE").transform.position;
	    modeObjectsAreResting = false;
		maximumObjectsSwitchInterval = 
		    Mathf.Max(12.0f, (18.0f / (int)(global.difficulty)));
		modeObjectsSwitch();
    }
	
	public void Start()
	{
        #if UNITY_EDITOR
        setThemeBasedOnScene();
        #endif
        
        global.currentArena = this;
	    paused = false;
        
		findReferencePoints();		
		preparePlayers();		
		setUpModeElements();
		setMusicVolume();
	    if (modeHasObjects) startModeObjectsCycle();
	        
	    global.ongoingGame = true;		
	}
	
	public void PlayPause()
	{
	    if ((global.now - lastPlayPauseTime).TotalMilliseconds < 200) return;
	    lastPlayPauseTime = global.now;
	    
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
	    if (modeHasObjects)
	    {
    	    if (paused) nextModeObjectsSwitch = 
	            nextModeObjectsSwitch.AddSeconds(Time.deltaTime);
	        else if (global.now > nextModeObjectsSwitch) modeObjectsSwitch();
	    }
	        
	    if (Input.GetKeyUp(KeyCode.Return)) PlayPause();    
	    else if ((!global.ongoingGame)) secretsHandler.readSecretCode();
	}
	
	public void finish( global.gameResult result, playerController winner = null )
	{
        global.ongoingGame = false;
        Object.Destroy(global.getByName("mode"));
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
