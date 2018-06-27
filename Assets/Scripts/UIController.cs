using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{   
    public static string currentPanel = "mainMenu";
    
    public static int whichTheme = global.lastClashTheme;
    public static int whichMode = global.lastClashMode;
    public static int[] whichCharacter = new int[] 
        {
            global.lastClashCharacters[0],
            global.lastClashCharacters[1],
            global.lastClashCharacters[2],
            global.lastClashCharacters[3]
        };
        
    public void changeTheme( string direction )
    { 
        int allowedCount = global.allowedArenaThemes.Count;
        
        if (direction == "next") whichTheme = (whichTheme + 1) % (allowedCount + 1);
        else whichTheme -= (whichTheme != 0) ? 1 : -allowedCount;
        
        if (whichTheme == 0) global.theme = global.randomArenaTheme();
        else global.theme = (global.arenaTheme)(global.allowedArenaThemes[whichTheme - 1]);
        
        Debug.Log(global.theme.ToString() + " theme selected");
    }
        
    public void changeMode( string direction )
    { 
        int allowedCount = global.allowedArenaModes.Count;
        
        if (direction == "next") whichMode = (whichMode + 1) % (allowedCount + 1);
        else whichMode -= (whichMode != 0) ? 1 : -allowedCount;
        
        if (whichMode == 0) global.mode = global.randomArenaMode();
        else global.mode = (global.arenaMode)(global.allowedArenaModes[whichMode - 1]);
        
        Debug.Log(global.mode.ToString() + " mode selected");
    }

    public void next( int which = 1 )
    {
        if (currentPanel == "clashCharactersPanel")
        {
            whichCharacter[which] = 
                (whichCharacter[which]++ % (global.allowedCharacters.Count + 1));
            Debug.Log("Clash: character " + whichTheme.ToString() + " selected" +
                "as player #" + which.ToString());
        }
    }

    public void previous( int which = 1 )
    {
        if (currentPanel == "clashCharactersPanel")
        {
            whichCharacter[which]--;
            if (whichCharacter[which] <= 0) 
                whichCharacter[which] = global.allowedCharacters.Count;
            Debug.Log("Clash: character " + whichTheme.ToString() + " selected [" +
                which.ToString() + "]");
        }
    }
    
    public void nextPanel()
    {
        if (currentPanel == "mainMenu")
        {
            currentPanel = "clashThemePanel";
            global.getByName("clashThemePanel").SetActive(true);
            global.getByName("mainMenuPanel").SetActive(false);
            global.clashMode = true;
        }
        else if (currentPanel == "clashThemePanel")
        {
            currentPanel = "clashModePanel";
            global.getByName("clashModePanel").SetActive(true);
            global.getByName("clashThemePanel").SetActive(false);
        }
        else if (currentPanel == "clashModePanel")
        {
            currentPanel = "clashCharactersPanel";
            global.getByName("clashCharactersPanel").SetActive(true);
            global.getByName("clashModePanel").SetActive(false);
        }
        else if (currentPanel == "clashCharactersPanel")
            startClash();
    }
    
    public void previousPanel()
    {
        if (currentPanel == "clashThemePanel")
        {
            currentPanel = "mainMenu";
            global.getByName("mainMenuPanel").SetActive(true);
            global.getByName("clashThemePanel").SetActive(false);
            global.clashMode = false;
        }
        else if (currentPanel == "clashModePanel")
        {
            currentPanel = "clashThemePanel";
            global.getByName("clashThemePanel").SetActive(true);
            global.getByName("clashModePanel").SetActive(false);
        }
        else if (currentPanel == "clashCharactersPanel")
        {
            currentPanel = "clashModePanel";
            global.getByName("clashModePanel").SetActive(true);
            global.getByName("clashCharactersPanel").SetActive(false);
        }
    }
    
    public void startClash()
    {
        currentPanel = "mainMenu";
        global.loadProperArenaScene();
    }
}
