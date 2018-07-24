using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cutsceneController : MonoBehaviour {
    private global.arenaTheme stageToGo;
    public GameObject[] sceneToShow = new GameObject[5];
    // Use this for initialization
    void Start () {
        if (stageToGo == global.arenaTheme.Cars)
        {
            sceneToShow[0].SetActive(true);
        }
        if (stageToGo == global.arenaTheme.Humanoids)
        {
            sceneToShow[1].SetActive(true);
        }
        if (stageToGo == global.arenaTheme.Fantasy)
        {
            sceneToShow[2].SetActive(true);
        }
        if (stageToGo == global.arenaTheme.Chess)
        {
            sceneToShow[3].SetActive(true);
        }
     //   if (lastStage==beaten)
        {
      //      sceneToShow[4].SetActive(true);
        }

    }

// Update is called once per frame
void Update () {
		
	}
}
