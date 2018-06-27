using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum UIImageType : byte
    {
        theme = 4,
        mode = 5,
        player1 = 0,
        player2 = 1,
        player3 = 2,
        player4 = 3
    };
    
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

    public void next( int which = 1 )
    {
        GameObject replacedImage = null;
        string imageToLoad = "";
        
        if (currentPanel == "clashThemePanel")
        {                                                                          
            whichTheme = (whichTheme++ % global.allowedArenaThemes.Count) + 1;
            global.theme = (global.arenaTheme)(global.allowedArenaThemes[whichTheme]);
            Debug.Log("Clash: theme " + whichTheme.ToString() + " selected");
            replacedImage = global.getByName("themeImage");
            imageToLoad = "themePreview" + whichTheme.ToString();
        }
        else if (currentPanel == "clashModePanel")
        {
            whichMode = (whichMode++ % global.allowedArenaModes.Count) + 1;
            global.mode = (global.arenaMode)(global.allowedArenaModes[whichMode]);
            Debug.Log("Clash: mode " + whichMode.ToString() + " selected");
            //replacedImage = global.getByName("modeImage");
        }
        else if (currentPanel == "clashCharactersPanel")
        {
            whichCharacter[which] = 
                (whichCharacter[which]++ % global.allowedCharacters.Count) + 1;
            Debug.Log("Clash: character " + whichTheme.ToString() + " selected" +
                "as player #" + which.ToString());
            //replacedImage = global.getByName("player" + which.ToString() + "Image");
        }
        
        global.setImage(replacedImage, imageToLoad);
    }

    public void previous( int which = 1 )
    {
        GameObject replacedImage = null;
        string imageToLoad = ""                              ;
        
        if (currentPanel == "clashThemePanel")
        {
            whichTheme--;
            if (whichTheme <= 0) whichTheme = global.allowedArenaThemes.Count;
            Debug.Log("Clash: theme " + whichTheme.ToString() + " selected");
            replacedImage = global.getByName("themeImage");
            imageToLoad = "themePreview" + whichTheme.ToString();
        }
        else if (currentPanel == "clashModePanel")
        {
            whichMode--;
            if (whichMode <= 0) whichMode = global.allowedArenaModes.Count;
            Debug.Log("Clash: mode " + whichMode.ToString() + " selected");
            //replacedImage = global.getByName("modeImage");
        }
        else if (currentPanel == "clashCharactersPanel")
        {
            whichCharacter[which]--;
            if (whichCharacter[which] <= 0) 
                whichCharacter[which] = global.allowedCharacters.Count;
            Debug.Log("Clash: character " + whichTheme.ToString() + " selected [" +
                which.ToString() + "]");
            //replacedImage = global.getByName("player" + which.ToString() + "Image");
        }
        
        global.setImage(replacedImage, imageToLoad);
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
