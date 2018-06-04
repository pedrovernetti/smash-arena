using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathPlane : MonoBehaviour
{
    public Text deathText;
    
    private arena arenaController;
    
    private string[] ranking;
    private int activePlayersCount;

	public void Start ()
	{
	    arenaController = GameObject.Find("arena").GetComponent<arena>();
	    if (arenaController == null) 
	        Debug.Log("scene is missing an arena object!");
		activePlayersCount = global.playersCount;
		ranking = new string[activePlayersCount];
	}
	
	private void setWinner()
	{
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerController winner;
        for (int i = 0; i < 4; i++)
        {
            if (players[i].active)
            {
                winner = players[i].GetComponent<playerController>();
	            ranking[0] = winner.playerName;
                if (players[i].GetComponent<playerController>().isGroundless)
                {
                    deathText.text = "DRAW";
                    arenaController.finish(global.gameResult.DRAW);
                }
                else
                {
                    deathText.text = winner + "\nWIN";
                    arenaController.finish(global.gameResult.WIN, winner);
                }
                return;
            }
        }
	}
	
	private void setDead( playerController controller )
	{
	    ranking[activePlayersCount - 1] = controller.playerName;
	    if ((controller.playerNumber == 1) && (!global.clashMode))
	    {
	        deathText.text = "YOU LOSE";
	        arenaController.finish(global.gameResult.LOSE);
	    }
	}
    
    public void OnTriggerEnter( Collider other )
    {
        other.gameObject.SetActive(false);
        if (other.CompareTag("Player"))
        {
            activePlayersCount--;
            setDead(other.gameObject.GetComponent<playerController>());
            if (activePlayersCount == 1) setWinner();
        }
    }
}
