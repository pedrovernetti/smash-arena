using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class global 
{
    public class Tuple<type1, type2>
    {
        public type1 Item1 { get; private set; }
        public type2 Item2 { get; private set; }

        public Tuple(type1 item1, type2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public enum arenaMode : byte
    {
        Normal = 1,     // OK // nothing special
        Inverted = 2,   // arena has a hole in its center and walls by the sides
        Frozen = 3,     // ice objects - briefly slow characters down
        Burning = 4,    // fire objects - characters go briefly out of control
        Electric = 5,   // paralyzing shock objects
        Meteors = 6,    // stones fall randomly on the arena
        Unstable = 7,   // arena can tilt and fall
        Teleport = 8,   // teleporter objects (like Bomberman holes)
        Ghost = 9,      // characters almost invisible and randomly non-solid
        Funny = 10,     // messed physics, messed textures/colors, messed music
        Shrunken = 11,  // OK // characters are half their size
        Enlarged = 12,  // OK // characters are twice their size
        Dark = 13,      // OK // lighting is almost black
        DownsideUp = 14 // camera is rotated 180 around Z axis
    };  
    
    public enum arenaTheme : byte
    {
        Cars = 1,       // characters are cars
        Humanoids = 2,  // characters are humanoid warriors/soldiers
        Fantasy = 3,    // characters are fantasy mobile game monsters
        Chess = 4,      // characters are chess pieces
        Abstract = 5,   // characters are basic 3d geometric forms
        Secret = 6      // characters are wizard and bizarre creatures
    };
    
    public enum difficultyLevel : byte
    {
        Easy = 1,
        Normal = 2,
        Hard = 3,
        Hell = 4
    };
    
    public enum gameResult : byte
    {
        WIN = 1,
        DRAW = 2,
        LOSE = 4
    }
    
    public enum playerType : byte
    {
        Disabled = 0,
        Human = 1,
        Machine = 2,
        BrainDead = 4
    }

    // Settings
    
    public static float audioVolume = 1.0f;
    public static float musicVolume = 0.5f;
    public static bool fullscreen = false;
    
    // Game
    
    public static difficultyLevel difficulty = difficultyLevel.Normal;
    public static int difficultyFactor { get { return (int)(difficulty); } }
    
    public static arenaMode mode = (arenaMode)(1);
    public static arenaTheme theme = (arenaTheme)(1);
    
    public static bool bossEncounter = false;
    
    public static string[] playerNames = 
        new string[] 
        {
            "Player A",
            "Player B", 
            "Player C",
            "Player D"
        };
    public static string[] playerCharacters = 
        new string[] 
        {
            "missingNo1",
            "missingNo2",
            "missingNo3",
            "missingNo4"
        };
    public static playerType[] playerTypes = 
        new playerType[] 
        {
            playerType.Human,
            #if UNITY_EDITOR
            playerType.Machine,
            playerType.Machine,
            playerType.Machine
            #else
            playerType.Human,
            playerType.Human,
            playerType.Human
            #endif
        };
    
    public static bool ongoingGame = false;
    
    public static System.DateTime now { get { return System.DateTime.Now; } }
    public static bool coinflip { get { return (Random.value > 0.5f); } }
    
    public static string currentScene { get { return SceneManager.GetActiveScene().name; } }
    public static arena currentArena = null;
    public static ground currentArenaGround = null;
    
    // Clash-specific
    
    public static bool clashMode = false;
    public static int clashRounds = 5;
    public static int clashRoundsPlayed = 0;
    public static int[] clashVictories = 
        new int[] 
        {
            0, // Player A
            0, // Player B
            0, // Player C
            0  // Player D
        };
        
    // Progress and achievements
    
    public static int overallFinishedCampaigns = 0;
    
    public static SortedDictionary<difficultyLevel,int> finishedCampaigns = 
        new SortedDictionary<difficultyLevel,int>
        {
            { difficultyLevel.Easy,   0 },
            { difficultyLevel.Normal, 0 },
            { difficultyLevel.Hard,   0 },
            { difficultyLevel.Hell,   0 }
        };
        
    public static ArrayList allowedDifficultyLevels = 
        new ArrayList(
            new difficultyLevel[] 
            {
                difficultyLevel.Easy,
                difficultyLevel.Normal,
                difficultyLevel.Hard
            }
        );
        
    public static ArrayList allowedArenaThemes = 
        new ArrayList(
            new arenaTheme[] 
            {
                arenaTheme.Cars,
                arenaTheme.Humanoids,
                arenaTheme.Fantasy
            }
        );
        
    public static ArrayList allowedArenaModes = 
        new ArrayList(
            new arenaMode[] 
            {
                arenaMode.Normal,
                arenaMode.Inverted, 
                arenaMode.Frozen,
                arenaMode.Burning,
                arenaMode.Electric,
                arenaMode.Unstable
            }
        );
        
    public static ArrayList allowedCharacters = 
        new ArrayList(
            new string[] 
            {
                // cars
                "blueCar",
                "kendoVan",
                "newtonsCar",
                "oldTimer",
                // humanoids
                "akai",
                "newton",
                "whiteClown",
                "vampire",
                // fantasy
                "newtonGhost",
                "ghost",
                "rabbit",
                "slime",
                // chess
                "blackPawn",
                "blackRook",
                "blackKnight",
                "blackBishop"
            }
        );
        
    // Enemies lists by theme
    
    private static string lastPickedEnemy = null;
        
    public static Tuple<string, float>[] carEnemies = 
        new Tuple<string, float>[] 
        {
            new Tuple<string, float>("blueCar", 25.0f),
            new Tuple<string, float>("forklift", 20.0f),
            new Tuple<string, float>("kendoVan", 15.0f),
            new Tuple<string, float>("humvee", 15.0f),
            new Tuple<string, float>("stationWagon", 15.0f),
            new Tuple<string, float>("taxi", 15.0f),
            new Tuple<string, float>("oldTimer", 5.0f)
        };
        
    public static Tuple<string, float>[] humanoidEnemies = 
        new Tuple<string, float>[] 
        {
            new Tuple<string, float>("akai", 20.0f),
            new Tuple<string, float>("heraklios", 10.0f),
            //new Tuple<string, float>("knight", 10.0f),
            new Tuple<string, float>("nightShade", 10.0f),
            //new Tuple<string, float>("spartanHero", 20.0f),
            new Tuple<string, float>("vampire", 10.0f),
            new Tuple<string, float>("whiteClown", 10.0f),
            new Tuple<string, float>("wiezzorek", 10.0f),
            new Tuple<string, float>("yBot", 2.5f)
        };
        
    public static Tuple<string, float>[] fantasyEnemies = 
        new Tuple<string, float>[] 
        {
            new Tuple<string, float>("ghost", 20.0f),
            new Tuple<string, float>("rabbit", 20.0f),
            new Tuple<string, float>("footman", 25.0f),
            new Tuple<string, float>("impling", 15.0f),
            new Tuple<string, float>("golem", 3.0f),
            new Tuple<string, float>("slime", 20.0f)
        };
        
    public static Tuple<string, float>[] chessEnemies = 
        new Tuple<string, float>[] 
        {
            new Tuple<string, float>("blackPawn", 37.0f),
            new Tuple<string, float>("blackRook", 20.0f),
            new Tuple<string, float>("blackKnight", 15.0f),
            new Tuple<string, float>("blackBishop", 15.0f),
            new Tuple<string, float>("blackQueen", 1.0f),
            new Tuple<string, float>("whitePawn", 31.0f),
            new Tuple<string, float>("whiteRook", 20.0f),
            new Tuple<string, float>("whiteKnight", 5.0f),
            new Tuple<string, float>("whiteBishop", 5.0f),
            new Tuple<string, float>("whiteQueen", 1.0f)
        };
        
    // Functions to manipulate things using tags or names
    
    public static GameObject getByName( string name )
    {
        GameObject parent = GameObject.Find("arena");
        if (parent == null) parent = GameObject.Find("mainMenu");
        Transform[] all = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in all)
        {
            if (transform.name == name) return transform.gameObject;
        }
        return null;
    }
    public static GameObject getChildByName( GameObject parent, string childName )
    {
        Transform[] all = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in all)
        {
            if (transform.name == childName) return transform.gameObject;
        }
        return null;
    }
    
    public static GameObject[] getByTag( string tag )
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }
    
    public static void setActiveByTag( string tag, bool active = true )
    {
        GameObject[] stuff = global.getByTag(tag);
        for (int i = stuff.Length - 1; i >= 0; i--)
            stuff[i].SetActive(active);
    }
    
    public static bool isMachineControlled( int playerNumber )
    {
        if ((!clashMode) && (playerNumber > 1)) 
            return true;
        else if (playerTypes[playerNumber] == playerType.Machine)
            return true;
        else return false;
    }
    
    public static bool isMachineControlled( GameObject player )
    {
        playerController controller = player.GetComponent<playerController>();
        return isMachineControlled(controller.playerNumber);
    }
    
    // Audio functions
    
    public static void playClip( AudioClip clip )
    {
        Vector3 where = getByName("camera").transform.position;
        AudioSource.PlayClipAtPoint(clip, where, audioVolume);
    }
    
    public static void playClipAt( AudioClip clip, Vector3 where, float volume )
    {
        AudioSource.PlayClipAtPoint(clip, where, volume * audioVolume);
    }
    
    // Game control functions
    
    public static bool chance( float percent )
    {
        return (Random.Range(0.0f, 100.0f) < percent);
    }
    
    public static float noiseFreeValue( float x )
    {
        if ((x < 0.05f) && (x > -0.05f)) x = 0.0f;
        return x;
    }
    
    public static int playersCount()
    {
        int count = 0;
	    for (int i = 0; i < 4; i++)
	    {
	        if (playerTypes[i] != playerType.Disabled) count++;
	    }
	    return count;
    }
    
    public static arenaTheme randomArenaTheme()
    {
        int which = Random.Range(0, (allowedArenaThemes.Count - 1));
        return (arenaTheme)(allowedArenaThemes[which]);
    }
    
    public static arenaTheme allowedArenaTheme( int which )
    {
        if ((which == 0) || (which > allowedArenaThemes.Count))
            return randomArenaTheme();
        else return (arenaTheme)(allowedArenaThemes[which - 1]);
    }
    
    public static arenaMode randomArenaMode()
    {
        int which = Random.Range(0, (allowedArenaModes.Count - 1));
        if (((difficulty == difficultyLevel.Easy) || (theme == arenaTheme.Cars)) && 
            ((arenaMode)(allowedArenaModes[which]) == arenaMode.Unstable))
            return randomArenaMode();
        if ((theme == arenaTheme.Fantasy) &&
            ((arenaMode)(allowedArenaModes[which]) == arenaMode.Inverted))
            return randomArenaMode();
        return (arenaMode)(allowedArenaModes[which]);
    }
    
    public static arenaMode allowedArenaMode( int which )
    {
        if ((which == 0) || (which > allowedArenaModes.Count))
            return randomArenaMode();
        else return (arenaMode)(allowedArenaModes[which - 1]);
    }
    
    public static string mainCharacter()
    {
        if (theme == arenaTheme.Cars) return "newtonsCar";
        else if (theme == arenaTheme.Humanoids) return "newton";
        else if (theme == arenaTheme.Fantasy) return "newtonGhost";
        else return "chessNewton";
    }
    
    public static string randomCharacter()
    {
        Tuple<string, float> character;
        if (theme == arenaTheme.Cars)
            character = carEnemies[Random.Range(0, (carEnemies.Length - 1))];
        else if (theme == arenaTheme.Humanoids)
            character = humanoidEnemies[Random.Range(0, (humanoidEnemies.Length - 1))];
        else if (theme == arenaTheme.Fantasy)
            character = fantasyEnemies[Random.Range(0, (fantasyEnemies.Length - 1))];
        else character = chessEnemies[Random.Range(0, (chessEnemies.Length - 1))];
        
        if ((Random.Range(0, 100) < character.Item2) && 
            (character.Item1 != lastPickedEnemy))
        {
            lastPickedEnemy = character.Item1;
            return character.Item1;
        }
        else return randomCharacter();
    }
    
    public static string reallyRandomCharacter()
    {
        int which = Random.Range(0, (allowedCharacters.Count - 1));
        return (string)(allowedCharacters[which]);
    }
    
    public static string allowedCharacter( int which )
    {
        if ((which == 0) || (which > allowedCharacters.Count))
            return reallyRandomCharacter();
        else return (string)(allowedCharacters[which - 1]);
    }
    
    public static void goToMainMenu()
    {
        Debug.Log("Main Menu");
        if (SceneManager.GetActiveScene().name != "mainMenu")
            SceneManager.LoadScene("mainMenu");
        
        getByName("mainMenuPanel").SetActive(true);
        getByName("clashMode").SetActive(false);
        getByName("options").SetActive(false);
            
        bossEncounter = false;
        theme = (arenaTheme)(1);
        mode = (arenaMode)(1);
        clashMode = false;
        currentArena = null;
    }
    
    public static void goToClashModePanel()
    {
        Debug.Log("Clash Mode");
        if (SceneManager.GetActiveScene().name != "mainMenu")
            SceneManager.LoadScene("mainMenu");
            
        getByName("mainMenuPanel").SetActive(false);
        getByName("clashMode").SetActive(true);
        getByName("options").SetActive(false);
        
        bossEncounter = false;
        getByName("mainMenu").GetComponent<UIController>().changeTheme("clash");
        getByName("mainMenu").GetComponent<UIController>().changeMode("clash");
        clashMode = true;
        currentArena = null;
    }
    
    public static void setPlayers()
    {
        if (!clashMode)
        {
            if (bossEncounter)
            {
                global.playerCharacters[0] = global.mainCharacter();
                global.playerCharacters[1] = "boss";
                playerTypes = new playerType[] 
                    { playerType.Human, playerType.Machine, 
                      playerType.Disabled, playerType.Disabled };
            }
            else
            {
                playerCharacters[0] = mainCharacter();
	            for (int i = 1; i < 4; i++) playerCharacters[i] = randomCharacter();
                playerTypes = new playerType[] 
                    { playerType.Human, playerType.Machine, 
                      playerType.Machine, playerType.Machine };
	        }
	    }
	    Debug.Log("Players: " + playerCharacters[0] + ":" + playerTypes[0] + ", " + 
	                playerCharacters[1] + ":" + playerTypes[1] + ", " + 
	                playerCharacters[2] + ":" + playerTypes[2] + ", " + 
	                playerCharacters[3] + ":" + playerTypes[3]);
    }
    
    public static void loadProperArenaScene()
    {
        string scene = "mainMenu";
        
        if (bossEncounter) mode = arenaMode.Normal;
        else mode = randomArenaMode();
        
        if (theme == arenaTheme.Cars) scene = "cars";
        else if (theme == arenaTheme.Humanoids) scene = "humanoids";
        else if (theme == arenaTheme.Fantasy) scene = "fantasy";
        else if (theme == arenaTheme.Chess) scene = "chess";
        else if (theme == arenaTheme.Abstract) scene = "abstract";
        else if (theme == arenaTheme.Secret) scene = "secret";
        
        if (!clashMode) setPlayers();
        currentArena = null;
	                
        Debug.Log("\"" + scene + "\" arena starting...");
        SceneManager.LoadScene(scene);
    }
    
    public static void restart()
    {
        Debug.Log("Reseting the game");
        
        mode = (arenaMode)(1);
        theme = (arenaTheme)(1);
        bossEncounter = false;
        ongoingGame = false;
        currentArena = null;
        currentArenaGround = null;
        
        clashMode = false;
        clashRoundsPlayed = 0;
        clashVictories[0] = clashVictories[1] = 
                clashVictories[2] = clashVictories[3] = 0;
                
        goToMainMenu();
    }
    
    public static void quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    public static void advanceCampaign()
    {
        if (theme == arenaTheme.Chess)
        {
            if (!bossEncounter) bossEncounter = true;
            else 
            {
                beatTheGame();
                restart();
                goToMainMenu();
                return;
            }
        }
        else theme++;
        // cutscene
        loadProperArenaScene();
    }
    
    private static void unlockStuff()
    {
        if ((difficulty >= difficultyLevel.Normal) && 
            (overallFinishedCampaigns == 0) && (theme == arenaTheme.Chess))
            allowedArenaThemes.Add(arenaTheme.Chess);
        if ((!allowedArenaThemes.Contains(arenaTheme.Abstract)) &&
            (theme == arenaTheme.Abstract))
            allowedArenaThemes.Add(arenaTheme.Abstract);
        if ((!allowedArenaThemes.Contains(arenaTheme.Secret)) &&
            (theme == arenaTheme.Secret))
            allowedArenaThemes.Add(arenaTheme.Secret);
                
        /*if (finishedCampaigns[difficultyLevel.Easy] == 1)
            // unlock ridiculous character
        else*/ if (finishedCampaigns[difficultyLevel.Easy] == 4)
            allowedArenaModes.Add(arenaMode.Funny);
            
        if (finishedCampaigns[difficultyLevel.Normal] == 2)
            allowedArenaModes.Add(arenaMode.Teleport);
            
        if (finishedCampaigns[difficultyLevel.Hard] == 1)
            allowedDifficultyLevels.Add(difficultyLevel.Hell);
        else if (finishedCampaigns[difficultyLevel.Hard] == 2)
            allowedArenaModes.Add(arenaMode.Ghost);
        else if (finishedCampaigns[difficultyLevel.Hard] == 3)
            allowedArenaModes.Add(arenaMode.Meteors);
            
        if (finishedCampaigns[difficultyLevel.Normal] == 1)
            allowedDifficultyLevels.Add(difficultyLevel.Hell);
        else if (finishedCampaigns[difficultyLevel.Normal] == 2)
            allowedArenaModes.Add(arenaMode.Shrunken);
        else if (finishedCampaigns[difficultyLevel.Normal] == 3)
            allowedArenaModes.Add(arenaMode.Enlarged);
    }
    
    private static void beatTheGame()
    {
        // chamar cutscene final.. mais alguma coisa
        
        overallFinishedCampaigns++;
        
        if (difficulty == difficultyLevel.Normal)
            finishedCampaigns[difficultyLevel.Normal]++;
        else if (difficulty == difficultyLevel.Hard)
            finishedCampaigns[difficultyLevel.Hard]++;
        else if (difficulty == difficultyLevel.Hell)
            finishedCampaigns[difficultyLevel.Hell]++;
        else // difficultyLevel.Easy
            finishedCampaigns[difficultyLevel.Easy]++;
            
        Debug.Log
        (
            "Game beaten. [x" + 
            finishedCampaigns[difficultyLevel.Easy].ToString() + ", x" +
            finishedCampaigns[difficultyLevel.Normal].ToString() + ", x" +
            finishedCampaigns[difficultyLevel.Hard].ToString() + ", x" +
            finishedCampaigns[difficultyLevel.Hell].ToString() + "]"
        );
            
        unlockStuff();        
        goToMainMenu();
    }
}
