using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{   
    public static int whichTheme = 0;
    public static int whichMode = 0;
    public static int[] whichCharacter = new int[] { 0, 0, 0, 0 };
    
    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "mainMenu")
        {
            global.getByName("mainMenuPanel").SetActive(true);
            global.getByName("clashThemePanel").SetActive(true);
            global.getByName("clashMode").SetActive(false);
            global.getByName("options").SetActive(false);
        }
    }
        
    public void changeTheme( string direction )
    { 
        int allowedCount = global.allowedArenaThemes.Count;        
        if (direction == "previous") whichTheme -= (whichTheme != 0) ? 1 : -allowedCount;
        else if (direction == "next") whichTheme = (whichTheme + 1) % (allowedCount + 1);
        
        global.theme = global.allowedArenaTheme(whichTheme);        
        Debug.Log(global.theme.ToString() + " theme selected");
    }
        
    public void changeMode( string direction )
    { 
        int allowedCount = global.allowedArenaModes.Count;        
        if (direction == "previous") whichMode -= (whichMode != 0) ? 1 : -allowedCount;
        else if (direction == "next") whichMode = (whichMode + 1) % (allowedCount + 1);
        
        global.mode = global.allowedArenaMode(whichMode);        
        Debug.Log(global.mode.ToString() + " mode selected");
    }

    public void next( int which = 1 )
    {
        whichCharacter[which] = 
            (whichCharacter[which]++ % (global.allowedCharacters.Count + 1));
        Debug.Log("Clash: character " + whichTheme.ToString() + " selected" +
            "as player #" + which.ToString());
    }

    public void previous( int which = 1 )
    {
        whichCharacter[which]--;
        if (whichCharacter[which] <= 0) 
            whichCharacter[which] = global.allowedCharacters.Count;
        Debug.Log("Clash: character " + whichTheme.ToString() + " selected [" +
            which.ToString() + "]");
    }
    
    public void goToClashModePanel()
    {
        global.goToClashModePanel();
    }
    
    public void goToMainMenu()
    {
        global.goToMainMenu();
    }
    
    public void startGame()
    {
        global.loadProperArenaScene();
    }
    
    public void quit()
    {
        global.quit();
    }
}
