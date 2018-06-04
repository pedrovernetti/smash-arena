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
        global.setActiveByTag("fire", global.activeness.False);
        global.setActiveByTag("ice", global.activeness.False);
        global.setActiveByTag("shock", global.activeness.False);
	}

	private void invertedModeChanges()
	{
	    useInvertedArenaGround(true);
	}
	
	private void frozenModeChanges()
	{
	    useInvertedArenaGround(false);
        setLightingColor(new Color(0.72F, 0.92F, 1.0F, 1.0F));
        global.setActiveByTag("ice", global.activeness.Random);
	}
	
	private void burningModeChanges()
	{
	    useInvertedArenaGround(false);
        setLightingColor(new Color(1.0F, 0.89F, 0.7F, 1.0F));
        global.setActiveByTag("fire", global.activeness.Random);
	}
	
	private void electricModeChanges()
	{
	    useInvertedArenaGround(false);
        global.setActiveByTag("shock", global.activeness.Random);
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
		    
		if (global.theme == global.arenaTheme.Chess)
		{
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
	}
	
	public void finish( global.gameResult result, playerController winner = null )
	{
        global.ongoingGame = false;
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
