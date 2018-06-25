using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    int whichTheme;
    int[] whichCharacter;
    int whichMode;

    public void Start()
    {
        whichCharacter = new int[4];
    }

    public void next( int which = 1 )
    {
        if (GameObject.Find("clashModePane").activeInHierarchy)
        {
            whichMode = ((whichMode++) % 11) + 1;
            GameObject imageObject = GameObject.Find("modeImage");
            if (image != null) imageObject.GetComponent<Image>().sprite = ;
        }
        else if (GameObject.Find("clashThemePane").activeInHierarchy)
        {
            whichTheme = (whichMode++) % 4;
            GameObject imageObject = GameObject.Find("themeImage");
            if (image != null) imageObject.GetComponent<Image>().sprite = ;
        }
        else if (GameObject.Find("clashCharacterPane").activeInHierarchy)
        {
            whichCharacter[which] = (whichMode++) % 4;
            GameObject imageObject = GameObject.Find("player" + which.ToString() + "Img");
            if (image != null) imageObject.GetComponent<Image>().sprite = ;
        }
    }

    public void previous()
    {

    }


}
