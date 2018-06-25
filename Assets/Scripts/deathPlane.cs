using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathPlane : MonoBehaviour
{
    public Text deathText;
    public Text roundText;
    private arena arenaController;
    
    private string[] ranking;
    private int activePlayersCount;

	public void Start ()
	{
	    arenaController = GameObject.Find("arena").GetComponent<arena>();
	    if (arenaController == null) 
	        Debug.Log("scene is missing an arena object!");
		activePlayersCount = (global.bossEncounter) ? 2 : global.playersCount;
		ranking = new string[activePlayersCount];
	}
	
	public IEnumerator showDeathText( string text, float delay = 4f )
	{
        deathText.text = text;
        yield return new WaitForSeconds(delay);
        deathText.text = "";
	}
   
    private void setWinner()
	{
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerController winner;
        for (int i = 0; i < 4; i++)
        {
            if (players[i].activeInHierarchy)
            {
                winner = players[i].GetComponent<playerController>();
	            ranking[0] = winner.playerName;
                if (winner.isGroundless)
                {
	                StartCoroutine(showDeathText("DRAW"));
                    arenaController.finish(global.gameResult.DRAW);
                }
                else if (winner.isCampaignPlayer)
                {
	                StartCoroutine(showDeathText("YOU WIN"));
                    arenaController.finish(global.gameResult.WIN, winner);
                }
                else
                {
	                StartCoroutine(showDeathText(winner.playerName + "\nWINS"));
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
	        StartCoroutine(showDeathText("YOU LOSE"));
	        arenaController.finish(global.gameResult.LOSE);
	    }
	    else if (global.clashMode)
	        StartCoroutine(showDeathText(controller.playerName + "\nDIES", 2f));
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
