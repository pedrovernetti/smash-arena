﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class arena : MonoBehaviour
{
    public enum arenaGroundShape : byte
    {
        Round = 1,
        Square = 2
    }
    
    public class rankingBuilder : IComparer
    {
        public int Compare( object A, object B )
        {
            return (((global.Tuple<string, int>)A).Item2.
                        CompareTo(((global.Tuple<string, int>)B).Item2));
        }
    }
    
    #if UNITY_EDITOR
    public static bool quickTest = true;
    #endif
    
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
    
    private int activePlayersCount;
    public Text roundText;
    public Text deathText;
    private System.DateTime textExpiration;
    public AudioClip deathSound;

    private System.DateTime lastPlayPauseTime;
    private bool paused;
    public bool isPaused { get { return paused; } }
    
    private string exitAction;
    
    private void setAsCurrentArena()
    {
	    if (global.currentArena != null) Object.Destroy(global.currentArena);
	    global.currentArena = this;
	}
    
    #if UNITY_EDITOR
    private void editorModeWorkaround()
    {
        if (!quickTest) return;        
        Debug.Log("#QuickTesting");
        
        secretsHandler.unlockEverything();
        
        if (global.currentScene == "cars") 
            global.theme = global.arenaTheme.Cars;
        else if (global.currentScene == "humanoids")
            global.theme = global.arenaTheme.Humanoids;
        else if (global.currentScene == "fantasy")
            global.theme = global.arenaTheme.Fantasy;
        else if (global.currentScene == "chess")
            global.theme = global.arenaTheme.Chess;
        
        if (!global.bossEncounter && !global.clashMode)
            global.mode = global.randomArenaMode();
            
        Debug.Log("(" + global.mode + ")");
        
        quickTest = false;
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
	
	public Vector3 unsafeRandomArenaPosition( float y )
	{
    	float x, z;
		if (arenaShape == arenaGroundShape.Square)
		{
		    x = Random.Range(referencePoint.x, (referencePoint.x + arenaSizeX));
		    z = Random.Range(referencePoint.z, (referencePoint.z + arenaSizeZ));
		}
		else
		{
		    x = referencePoint.x + (Random.Range(-arenaRadius, arenaRadius) * 0.9f);
		    z = referencePoint.z + (Random.Range(-arenaRadius, arenaRadius) * 0.9f);
		}
		return (new Vector3(x, y, z));
	}
	
	public Vector3 randomArenaPosition( float y )
	{
	    Vector3 position = unsafeRandomArenaPosition(y);
	    while (!isInsideArenaLimits(position))
	    {
	        position = unsafeRandomArenaPosition(y);
	    }
		return position;
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
        global.setActiveByTag("onlyUnstableMode", false);
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
		    invertedModeChanges();
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
	
	private void setSoundVolume()
	{
	    GameObject music = global.getByName("soundtrack");
	    if (music != null) 
	        music.GetComponent<AudioSource>().volume = global.musicVolume;
	    
        Transform[] all = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in all)
        {
            if ((transform.gameObject.GetComponent<AudioSource>() != null) &&
                (transform.name != "soundtrack"))
                {
                    transform.gameObject.GetComponent<AudioSource>().volume =
                        global.audioVolume;
                }
        }
	}
	
	public void hideObject( GameObject something )
	{
	    something.transform.position = objectsRestingPlace;
	    if (something.GetComponent<AudioSource>() != null)
	        something.GetComponent<AudioSource>().Pause();
	}
	
	public void modeObjectsSwitch()
	{
	    nextModeObjectsSwitch =
	        global.now.AddSeconds(Random.Range(2.0f, maximumObjectsSwitchInterval));
	    if (modeObjectsAreResting)
	    {
	        modeObjectsAreResting = false;
	        if (global.ongoingGame) foreach (GameObject modeObject in modeObjects)
	        {
	            modeObject.SetActive(true);
	            modeObject.transform.position = 
	                unsafeRandomArenaPosition(objectsRestingPlace.y);
	            if (modeObject.GetComponent<AudioSource>() != null)
	                modeObject.GetComponent<AudioSource>().Play();
	        }
	    }
	    else
	    {
	        modeObjectsAreResting = true;
	        if (global.ongoingGame) foreach (GameObject modeObject in modeObjects)
	        {
	            hideObject(modeObject);
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
		    Mathf.Max(12.0f, (18.0f / global.difficultyFactor));
		modeObjectsSwitch();
    }
	
	public void showText( string text, float duration, bool useDeathText = false )
	{
	    if (useDeathText)
	    {
	        roundText.text = "";
	        deathText.text = text;
	    }
	    else
	    {
	        deathText.text = "";
	        roundText.text = text;
	    }
        textExpiration = global.now.AddSeconds(duration);
	}
	
	public void Awake()
	{
        setAsCurrentArena();
        #if UNITY_EDITOR
        editorModeWorkaround();
        #endif
        
	    paused = false;
	}
	
	public void Start()
	{
	    if (global.clashMode) 
	        showText("round " + (global.clashRoundsPlayed + 1), 3f);
        
		findReferencePoints();		
		setUpModeElements();
		setSoundVolume();
	    if (modeHasObjects) startModeObjectsCycle();
	    
	    activePlayersCount = global.playersCount();
	    
	    global.ongoingGame = true;
	}
	
	private void removeExpiredTexts()
	{
	    if (((deathText.text != "") || (roundText.text != "")) &&
	        (global.now >= textExpiration))
	        deathText.text = roundText.text = "";
	}
	
	public void PlayPause()
	{
	    if ((global.now - lastPlayPauseTime).TotalMilliseconds < 200) return;
	    lastPlayPauseTime = global.now;
	    
	    //GameObject[] objectsToPlayPause = ;
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
	    removeExpiredTexts();
	    
	    if (modeHasObjects)
	    {
    	    if (paused) nextModeObjectsSwitch = 
	            nextModeObjectsSwitch.AddSeconds(Time.deltaTime);
	        else if (global.now > nextModeObjectsSwitch) modeObjectsSwitch();
	    }
	        
	    if (Input.GetButton("PlayPause")) PlayPause();    
	    else if ((!global.ongoingGame) && 
	             (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.JoystickButton6))) 
	        global.goToMainMenu();
	    else if ((!global.ongoingGame)) 
	        secretsHandler.readSecretCode();
	}
	
	private void exitArena()
	{
	    if (global.clashMode) global.clashRoundsPlayed++;
	    if (global.clashRoundsPlayed >= global.clashRounds)
	    {
	    
	        exitAction = "end";
	    }
	    
	    if (exitAction == "end") 
	    {
	        global.restart();
	        global.goToMainMenu();
	    }
	    else if (exitAction == "next") global.advanceCampaign();
	    else global.loadProperArenaScene();
	}
	
	public void showClashRanking()
	{
	    ArrayList ranking = new ArrayList();
	    for (int i = 0, victories; i < 4; i++)
	    {
	        victories = global.clashVictories[i];
	        ranking.Add(new global.Tuple<string, int>(global.playerNames[i], victories));
	    }
	    ranking.Sort(new rankingBuilder());
	    showText(("1. " + ((global.Tuple<string, int>)(ranking[0])).Item1 + 
	             "\n2. " + ((global.Tuple<string, int>)(ranking[1])).Item1 + 
	             "\n3. " + ((global.Tuple<string, int>)(ranking[2])).Item1 + 
	             "\n4. " + ((global.Tuple<string, int>)(ranking[3])).Item1), 5);
	}
	
	public void invokeOnTextExpiration( string methodName )
	{
	    float time = (float)((textExpiration - global.now).TotalSeconds) + 0.5f;
	    Invoke(methodName, time);
	}
	
	public void finish( global.gameResult result, playerController winner = null )
	{
        global.ongoingGame = false;
        Debug.Log("End");
        Object.Destroy(global.getByName("mode"));
	    if (result == global.gameResult.WIN)
	    {
	        if (global.clashMode)
	        {
	            showText(winner.playerName + "\nWINS", 4, true);
                global.clashVictories[winner.playerNumber - 1]++;
	            if (global.clashRoundsPlayed < global.clashRounds)
	                exitAction = "repeat";
	            else
	            {
	                exitAction = "end";
	                invokeOnTextExpiration("showClashRanking");
	            }
	        }
	        else 
	        {
                showText("YOU WIN", 4, true);
	            exitAction = "next";
	        }
	    }
	    else if (result == global.gameResult.DRAW)
	    {
	        showText("DRAW", 4, true);
	        if (global.bossEncounter) 
	        {
                global.bossEncounter = false;
                exitAction = "end";
	        }
            else exitAction = "repeat";
	    }
	    else 
	    {
            global.playClip(deathSound);
            showText("YOU LOSE", 4, true);
            exitAction = "end";
	    }
	    invokeOnTextExpiration("exitArena");
	}
	
	private void setWinner()
	{
        GameObject[] players = global.getByTag("Player");
        playerController winner;
        for (int i = 0; i < activePlayersCount; i++)
        {
            if (players[i].activeInHierarchy && 
                isInsideArenaLimits(players[i].transform.position))
            {
                winner = players[i].GetComponent<playerController>();
                if (winner.isGroundless) finish(global.gameResult.DRAW);
                else finish(global.gameResult.WIN, winner);
                return;
            }
        }
        finish(global.gameResult.DRAW);
	}
	
	public void setDead( playerController player )
	{
	    activePlayersCount--;
	    
	    if (global.clashMode)
	        showText(player.playerName + "\nDIES", 2, true);
	    else  if ((player.playerNumber == 1) && (!global.clashMode))
            finish(global.gameResult.LOSE);
	    
	    if (activePlayersCount == 1) setWinner();
	}
}
