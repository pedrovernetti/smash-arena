using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class selectionImage : MonoBehaviour
{
    public enum imageType : byte
    {
        theme = 4,
        mode = 5,
        player1 = 0,
        player2 = 1,
        player3 = 2,
        player4 = 3
    };
    
    private int[] which;
    private int[] internalIndex;
    private GameObject[] possibilities;
    
    public imageType type;
        
    public void Start()
    {
        which = new int[] { 0, 0, 0, 0 };        
        internalIndex = new int[] { 0, 0, 0, 0 };        
        possibilities = new GameObject[transform.childCount];
        for (int i = possibilities.Length - 1; i >= 0; i--)
        {
            possibilities[i] = transform.GetChild(i).gameObject;
            if (i != 0) possibilities[i].SetActive(false);
            else possibilities[i].SetActive(true);
        }
    }
	
    public void FixedUpdate()
    {
        if ((type == imageType.theme) && (which[0] != UIController.whichTheme))
        {
            possibilities[internalIndex[0]].SetActive(false);
            which[0] = UIController.whichTheme;
            internalIndex[0] = (which[0] == 0) ? 0 : (int)(global.theme);
            possibilities[internalIndex[0]].SetActive(true);
        }
        else if ((type == imageType.mode) && (which[0] != UIController.whichMode))
        {
            possibilities[internalIndex[0]].SetActive(false);
            which[0] = UIController.whichMode;
            internalIndex[0] = (which[0] == 0) ? 0 : (int)(global.mode);
            possibilities[internalIndex[0]].SetActive(true);
        }
        /*else if ((type >= imageType.player1) && (type <= imageType.player4) &&
                 (whichCharacter[(int)(type)] !=
                    UIController.whichCharacter[(int)(type)]))
        {
            whichCharacter[(int)(type)] = UIController.whichCharacter[(int)(type)];
            //string imageToLoad = ;
        }*/
	} 
}
