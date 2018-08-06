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
    
    private static string scene;
    private static System.DateTime nextInputTime;
    private static bool showingClashCharactersPanel = false;
    
    public void funnyMess( bool complete = false )
    {
        GameObject gameLogo = global.getByName("gameLogo");
        if ((gameLogo != null) && (gameLogo.GetComponent<Rigidbody>() != null))
            gameLogo.GetComponent<Rigidbody>().useGravity = true;
    }
    
    public void Start()
    {
        scene = SceneManager.GetActiveScene().name;
        nextInputTime = global.now;
        GameObject UIElement;
        if (scene == "mainMenu")
        {
            global.getByName("mainMenuPanel").SetActive(true);
            
            showingClashCharactersPanel = false;
            global.getByName("clashThemePanel").SetActive(true);
            global.getByName("clashMode").SetActive(false);
            
            for (int i = 1; i <= 4; i++)
            {
                UIElement = global.getByName("difficulty" + i.ToString());
                if (global.difficulty != (global.difficultyLevel)(i))
                    UIElement.GetComponent<Toggle>().isOn = false;
                else UIElement.GetComponent<Toggle>().isOn = true;
            }
            global.getByName("musicVolumeSlider").GetComponent<Slider>().value =
                global.musicVolume;
            global.getByName("audioVolumeSlider").GetComponent<Slider>().value =
                global.audioVolume;
            global.getByName("options").SetActive(false);
            if (global.cheated) Invoke("funnyMess", 1.0f);
        }
        else if ((scene == "cars") || (scene == "humanoids") || (scene == "fantasy") ||
                 (scene == "chess") || (scene == "abstract") || (scene == "secret"))
        {
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
        Debug.Log(((whichTheme != 0) ? global.theme.ToString() : "Random") + " theme selected");
    }
        
    public void changeMode( string direction )
    { 
        int allowedCount = global.allowedArenaModes.Count;        
        if (direction == "previous") whichMode -= (whichMode != 0) ? 1 : -allowedCount;
        else if (direction == "next") whichMode = (whichMode + 1) % (allowedCount + 1);
        
        global.mode = global.allowedArenaMode(whichMode);
        if (whichMode != 0) global.modeWasAChoice = true;
        else global.modeWasAChoice = false;      
        Debug.Log(((whichMode != 0) ? global.mode.ToString() : "Random") + " mode selected");
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
    
    public void changePlayerType1( Text dropdownLabel = null )
    {
        changePlayerType(dropdownLabel.text, 0);
    }
    
    public void changePlayerType2( Text dropdownLabel = null )
    {
        changePlayerType(dropdownLabel.text, 1);
    }
    
    public void changePlayerType3( Text dropdownLabel = null )
    {
        changePlayerType(dropdownLabel.text, 2);
    }
    
    public void changePlayerType4( Text dropdownLabel = null )
    {
        changePlayerType(dropdownLabel.text, 3);
    }
    
    public void goToClashModePanel()
    {
        global.goToClashModePanel();
        global.setStartingClashPlayers();
    }
    
    public void showClashCharactersPanel( bool show )
    {
        showingClashCharactersPanel = show;
    }
    
    public void goToMainMenu()
    {
        global.goToMainMenu();
    }
    
    public void startGame( bool campaign = false )
    { 
        if (campaign) global.restart();
        global.loadProperArenaScene();
    }
    
    public void switchSecondButton()
    {
        if (global.currentArena == null) return;
        if ((global.getByName("quitButton") != null) &&
            (global.getByName("mainMenuButton") != null))
        {
            if (global.currentArena.isPaused)
            {
                global.getByName("mainMenuButton").SetActive(false);
                global.getByName("quitButton").SetActive(true);
            }
            else
            {
                global.getByName("quitButton").SetActive(false);
                global.getByName("mainMenuButton").SetActive(true);
            }
        }
    }
    
    public void playPauseGame()
    { 
        if (global.currentArena != null) global.currentArena.PlayPause();
    }
    
    public void quit()
    {
        global.quit();
    }
    
    public void Update()
    {
        if (scene != "mainMenu") return;
        if (global.now < nextInputTime) return;
        
        if (global.clashMode)
        {
            if (showingClashCharactersPanel)
            {
                if (global.verticalInput('A') > 0.0f) changeCharacter1("next");
                else if (global.verticalInput('A') < 0.0f) changeCharacter1("previous");
                if (global.verticalInput('B') > 0.0f) changeCharacter2("next");
                else if (global.verticalInput('B') < 0.0f) changeCharacter2("previous");
                if (global.verticalInput('C') > 0.0f) changeCharacter3("next");
                else if (global.verticalInput('C') < 0.0f) changeCharacter3("previous");
                if (global.verticalInput('D') > 0.0f) changeCharacter4("next");
                else if (global.verticalInput('D') < 0.0f) changeCharacter4("previous");
            }
            else
            {
                float h = global.horizontalInput();
                float v = global.verticalInput();
                if (h != 0.0f) changeTheme((h > 0.0f) ? "next" : "previous");
                if (v != 0.0f) changeMode((v > 0.0f) ? "next" : "previous");
            }
        }
        nextInputTime = global.now.AddSeconds(0.2f);
    }
}
