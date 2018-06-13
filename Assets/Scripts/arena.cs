using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arena : MonoBehaviour
{
    private System.DateTime lastPlayPauseTime;
    
    private void setLightingColor( Color color )
    {
        GameObject[] lightSources = global.getByTag("mainLightSource");
        lightSources[0].GetComponent<Light>().color = color;
    }
	
	private void normalModeChanges()
	{
        setLightingColor(new Color(1.0F, 1.0F, 1.0F, 1.0F));
        global.setActiveByTag("fire", false);
        global.setActiveByTag("ice", false);
        global.setActiveByTag("shock", false);
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
	
	private void ghostModeChanges()
	{
        Shader ghostShader = Shader.Find("Particles/Additive (Soft)");
	    foreach (GameObject x in global.getByTag("Player"))
	        x.GetComponent<Renderer>().material.shader = ghostShader;
	    foreach (GameObject x in global.getByTag("fakePlayer"))
	        x.GetComponent<Renderer>().material.shader = ghostShader;
        setLightingColor(new Color(0.5F, 0.5F, 0.5F, 1.0F));
	}
	
	public void Start()
	{
	    GameObject[] music = global.getByTag("music");
	    if (music[0] != null) 
	        music[0].GetComponent<AudioSource>().volume = global.musicVolume;
	        
	    global.ongoingGame = true;
	
		if (global.mode == global.arenaMode.Frozen)
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
	}
	
	public void FixedUpdate()
	{
	    if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Pause))
	    {
	        if ((global.now - lastPlayPauseTime).TotalMilliseconds > 200)
	        {
	            lastPlayPauseTime = global.now;
                global.ongoingGame = !global.ongoingGame;
		        secretsHandler.code = "";
		        Debug.Log((global.ongoingGame) ? "Resumed" : "Paused");
		    }
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
