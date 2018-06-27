using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIImageController : MonoBehaviour
{
    private int whichTheme;
    private int whichMode;
    private int[] whichCharacter;
    
    public UIController.UIImageType imageType;
        
    public void Start()
    {
        whichTheme = -1;
        whichMode = -1;
        whichCharacter = new int[] { -1, -1, -1, -1 };
    }
	
    public void FixedUpdate()
    {
        if ((imageType == UIController.UIImageType.theme) && 
            (whichTheme != UIController.whichTheme))
        {
            whichTheme = UIController.whichTheme;
            string imageToLoad = "arenaTheme" + ((int)(global.mode)).ToString();
            global.setImage(gameObject, imageToLoad);
        }
        else if ((imageType == UIController.UIImageType.mode) && 
                 (whichMode != UIController.whichMode))
        {
            whichMode = UIController.whichMode;
            string imageToLoad = "arenaMode" + ((int)(whichMode)).ToString();
            global.setImage(gameObject, imageToLoad);
        }
        else if ((imageType >= UIController.UIImageType.player1) && 
                 (imageType <= UIController.UIImageType.player4) &&
                 (whichCharacter[(int)(imageType)] !=
                    UIController.whichCharacter[(int)(imageType)]))
        {
            whichCharacter[(int)(imageType)] = 
                UIController.whichCharacter[(int)(imageType)];
            //string imageToLoad = ;
        }
	} 
}
