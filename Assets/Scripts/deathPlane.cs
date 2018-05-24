using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathPlane : MonoBehaviour
{
    public int activePlayersCount;
    
    public Text deathText;

	// Use this for initialization
	void Start ()
	{
		if ((activePlayersCount > 4) || (activePlayersCount < 1))
		    activePlayersCount = 4;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	private void showWinnerText()
	{
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        string winner;
        for (int i = 0; i < 4; i++)
        {
            deathText.text = i.ToString();
            if (players[i].active && !(players[i].GetComponent<playerController>().groundless))
            {
                winner = players[i].GetComponent<playerController>().playerName;
                deathText.text = winner + "\nWIN";
                return;
            }
        }
        deathText.text = "DRAW";
	}
	
	private void setDead( Component controller )
	{
	    
	}
    
    void OnTriggerEnter( Collider other )
    {
        other.gameObject.SetActive(false);
        if (other.CompareTag("Player"))
        {
            activePlayersCount--;
            if (activePlayersCount == 1) showWinnerText();
            else setDead(other.gameObject.GetComponent<playerController>());
        }
    }
}
