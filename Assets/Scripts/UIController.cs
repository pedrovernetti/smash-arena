using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
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
            
            GameObject tempToggle;
            for (int i = 1; i <= 4; i++)
            {
                tempToggle = global.getByName("difficulty" + i.ToString());
                if (global.difficulty != (global.difficultyLevel)(i))
                    tempToggle.GetComponent<Toggle>().isOn = false;
                else tempToggle.GetComponent<Toggle>().isOn = true;
            }
            global.getByName("musicVolumeSlider").GetComponent<Slider>().value =
                global.musicVolume;
            global.getByName("audioVolumeSlider").GetComponent<Slider>().value =
                global.audioVolume;
            global.getByName("options").SetActive(false);
        }
    }
    
    public void setAudioEffectsVolume( GameObject sliderObject )
    {
        global.audioVolume = sliderObject.GetComponent<Slider>().value;
        Debug.Log("Sound effects volume set to " + global.audioVolume.ToString());
    }
    
    public void setMusicVolume( GameObject sliderObject )
    {
        global.musicVolume = sliderObject.GetComponent<Slider>().value;
        Debug.Log("Music volume set to " + global.musicVolume.ToString());
    }
    
    public void setDifficulty( string difficultyLevel )
    {
        if (difficultyLevel == "easy")
            global.difficulty = global.difficultyLevel.Easy;
        else if (difficultyLevel == "hard")
            global.difficulty = global.difficultyLevel.Hard;
        else if (difficultyLevel == "hell")
            global.difficulty = global.difficultyLevel.Hell;
        else // difficultyLevel == "normal"
            global.difficulty = global.difficultyLevel.Normal;
        Debug.Log(global.difficulty.ToString() + " difficulty selected");
    }
    
    private void updateClashRoundsNumberOnGUI()
    {
        GameObject clashRoundsNumber = global.getByName("clashRoundsNumber");
        if (clashRoundsNumber.GetComponent<Text>() != null)
            clashRoundsNumber.GetComponent<Text>().text = global.clashRounds.ToString();
    }
    
    public void moreClashRounds()
    {
        if (global.clashRounds < 5) global.clashRounds++;
        updateClashRoundsNumberOnGUI();
        Debug.Log("Clash rounds set to " + global.clashRounds);
    }
    
    public void lessClashRounds()
    {
        if (global.clashRounds > 1) global.clashRounds--;
        updateClashRoundsNumberOnGUI();
        Debug.Log("Clash rounds set to " + global.clashRounds);
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
    
    public void changeCharacter( string direction, int whichPlayer )
    {
        int allowedCount = global.allowedCharacters.Count;
        if (direction == "previous") 
            whichCharacter[whichPlayer] -= 
                (whichCharacter[whichPlayer] != 0) ? 1 : -allowedCount;
        else if (direction == "next")
            whichCharacter[whichPlayer] = 
                (whichCharacter[whichPlayer] + 1) % (allowedCount + 1);
                
        for (int i = 0; i < 4; i++)
        {
            if ((i != whichPlayer) &&
                (global.allowedCharacter(whichCharacter[whichPlayer]) == 
                    global.playerCharacters[i]))
                changeCharacter(direction, whichPlayer);
        }
        
        global.playerCharacters[whichPlayer] = 
            global.allowedCharacter(whichCharacter[whichPlayer]);        
        Debug.Log(global.playerCharacters[whichPlayer] + " selected as player " + whichPlayer);
    }
    
    public void changeCharacter1( string direction )
    {
        changeCharacter(direction, 0);
    }
    
    public void changeCharacter2( string direction )
    {
        changeCharacter(direction, 1);
    }
    
    public void changeCharacter3( string direction )
    {
        changeCharacter(direction, 2);
    }
    
    public void changeCharacter4( string direction )
    {
        changeCharacter(direction, 3);
    }
    
    public void changePlayerType( string type, int whichPlayer )
    {
        if (type == "Human") 
            global.playerTypes[whichPlayer] = global.playerType.Human;
        else if (type == "Machine") 
            global.playerTypes[whichPlayer] = global.playerType.Machine;
        else if (type == "Disabled") 
            global.playerTypes[whichPlayer] = global.playerType.Disabled;
        else global.playerTypes[whichPlayer] = global.playerType.BrainDead;
        Debug.Log("Player " + whichPlayer + " set as " + global.playerTypes[whichPlayer]);
    }
    
    public void changePlayerType1( Text dropdownLabel )
    {
        changePlayerType(dropdownLabel.GetComponent<Text>().text, 0);
    }
    
    public void changePlayerType2( Text dropdownLabel )
    {
        changePlayerType(dropdownLabel.GetComponent<Text>().text, 1);
    }
    
    public void changePlayerType3( Text dropdownLabel )
    {
        changePlayerType(dropdownLabel.GetComponent<Text>().text, 2);
    }
    
    public void changePlayerType4( Text dropdownLabel )
    {
        changePlayerType(dropdownLabel.GetComponent<Text>().text, 3);
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
