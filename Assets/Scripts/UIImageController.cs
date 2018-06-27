using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIImageController : MonoBehaviour
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
    private GameObject[] possibilities;
    
    public imageType type;
        
    public void Start()
    {
        which = new int[] { 0, 0, 0, 0 };        
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
            possibilities[which[0]].SetActive(false);
            which[0] = UIController.whichTheme;
            possibilities[(which[0] == 0) ? 0 : (int)(global.theme)].SetActive(true);
        }
        else if ((type == imageType.mode) && (which[0] != UIController.whichMode))
        {
            possibilities[which[0]].SetActive(false);
            which[0] = UIController.whichMode;
            possibilities[(which[0] == 0) ? 0 : (int)(global.mode)].SetActive(true);
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
