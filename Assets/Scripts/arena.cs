using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arena : MonoBehaviour
{
    private void setLightingColor( Color color )
    {
        GameObject[] lightSources = global.getByTag("mainLightSource");
        lightSources[0].GetComponent<Light>().color = color;
    }
    
    private void useInvertedArenaGround( bool use )
    {
	    GameObject arena = GameObject.Find("arenaGround");
	    GameObject invertedArena = GameObject.Find("invertedArenaGround");
	    if (arena != null) arena.SetActive(!use);
	    if (invertedArena != null) invertedArena.SetActive(use);
    }
	
	private void normalModeChanges()
	{
	    useInvertedArenaGround(false);
        setLightingColor(new Color(1.0F, 1.0F, 1.0F, 1.0F));
        global.setActiveByTag("fire", false);
        global.setActiveByTag("ice", false);
        global.setActiveByTag("shock", false);
	}

	private void invertedModeChanges()
	{
	    useInvertedArenaGround(true);
	}
	
	private void frozenModeChanges()
	{
	    useInvertedArenaGround(false);
        setLightingColor(new Color(0.72F, 0.92F, 1.0F, 1.0F));
        global.setRandomlyActiveByTag("ice");
	}
	
	private void burningModeChanges()
	{
	    useInvertedArenaGround(false);
        setLightingColor(new Color(1.0F, 0.89F, 0.7F, 1.0F));
        global.setRandomlyActiveByTag("fire");
	}
	
	private void electricModeChanges()
	{
	    useInvertedArenaGround(false);
        global.setRandomlyActiveByTag("shock");
	}
	
	private void unstableModeChanges()
	{
	    useInvertedArenaGround(false);
	}
	
	private void shrunkenModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
	    foreach (GameObject x in players)
	    {
	        x.GetComponent<Transform>().localScale *= 0.5f;
	        x.GetComponent<Rigidbody>().mass *= 0.5f;
	    }
	    useInvertedArenaGround(false);
	}
	
	private void ghostModeChanges()
	{
        GameObject[] players = global.getByTag("Player");
        GameObject[] fakePlayers = global.getByTag("fakePlayer");
        Shader ghostShader = Shader.Find("Particles/Additive (Soft)");
	    foreach (GameObject x in players)
	        x.GetComponent<Renderer>().material.shader = ghostShader;
	    foreach (GameObject x in fakePlayers)
	        x.GetComponent<Renderer>().material.shader = ghostShader;
	    useInvertedArenaGround(false);
        setLightingColor(new Color(0.5F, 0.5F, 0.5F, 1.0F));
	}
	
	public void Start()
	{
	    GameObject[] music = global.getByTag("music");
	    if (music[0] != null) 
	        music[0].GetComponent<AudioSource>().volume = global.musicVolume;
	        
	    global.ongoingGame = true;
	
		if (global.mode == global.arenaMode.Inverted)
		    invertedModeChanges();
		else if (global.mode == global.arenaMode.Frozen)
		    frozenModeChanges();
		else if (global.mode == global.arenaMode.Burning)
		    burningModeChanges();
		else if (global.mode == global.arenaMode.Electric)
		    electricModeChanges();
		else if (global.mode == global.arenaMode.Shrunken)
		    shrunkenModeChanges();
		else if (global.mode == global.arenaMode.Ghost)
		    ghostModeChanges();
		else normalModeChanges();
		    
		if (global.bossEncounter)
	    {
            GameObject boss = GameObject.Find("boss");
            GameObject player2 = GameObject.Find("player2");
            GameObject player3 = GameObject.Find("player3");
            GameObject player4 = GameObject.Find("player4");
            if (boss != null) boss.SetActive(true);
            if (player2 != null) player2.SetActive(false);
            if (player3 != null) player3.SetActive(false);
            if (player4 != null) player4.SetActive(false);
        }
        else
        {
            GameObject boss = GameObject.Find("boss");
            if (boss != null) boss.SetActive(false);
        }
	}
	
	public IEnumerator finish( global.gameResult result, playerController winner = null )
	{
	    GameObject[] floatingStuff = global.getByTag("floating");
	    Rigidbody tempBody;
	    foreach (GameObject x in floatingStuff)
	    {
	        tempBody = x.GetComponent<Rigidbody>();
	        tempBody.useGravity = true;
	        tempBody.AddForce(Physics.gravity * tempBody.mass * 3);
	        orbit script = GetComponent<orbit>(); 
            if (script != null) script.enabled = false;
	    }
	        
        global.ongoingGame = false;
        yield return new WaitForSeconds(4);
	    if (result == global.gameResult.WIN)
	    {
	        if (global.clashMode)
	        {
	            global.clashVictories[winner.playerNumber]++;
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
