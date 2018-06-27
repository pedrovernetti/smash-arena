using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class global 
{
    public enum arenaMode : byte
    {
        Normal = 1,     // nothing special
        Inverted = 2,   // arena has a hole in its center and walls by the sides
        Frozen = 3,     // ice objects - briefly slow characters down
        Burning = 4,    // fire objects - characters go briefly out of control
        Electric = 5,   // paralyzing shock objects
        Meteors = 6,    // stones fall randomly on the arena
        Unstable = 7,   // arena can tilt and fall
        Teleport = 8,   // teleporter objects (like Bomberman holes)
        Ghost = 9,      // characters almost invisible and randomly non-solid
        Funny = 10,     // messed physics, messed textures/colors, messed music
        Shrunken = 11,  // characters are half their size
        Enlarged = 12,  // characters are twice their size
        Dark = 13       // lighting is almost nonexisting
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

    // Settings
    
    public static float audioVolume = 1;
    public static float musicVolume = 0;
    public static bool fullscreen = false;
    
    private static difficultyLevel gameDifficultyLevel = difficultyLevel.Normal;  
      
    public static difficultyLevel difficulty
    {
        get { return gameDifficultyLevel; }
        set
        {
            gameDifficultyLevel = value;
            if (gameDifficultyLevel == difficultyLevel.Easy) 
                allowedArenaModes.Remove(arenaMode.Unstable);
            else allowedArenaModes.Add(arenaMode.Unstable);
        }
    }
    
    // Game
    
    public static bool ongoingGame = false;
    
    public static System.DateTime now { get { return System.DateTime.Now; } }
    
    public static bool clashMode = false;
    public static int clashRounds = 5;
    public static int clashRoundsPlayed = 0;
    public static int[] clashVictories = new int[] 
        {
            0, // Player A
            0, // Player B
            0, // Player C
            0  // Player D
        };
        
    public static int lastClashTheme = 1;
    public static int lastClashMode = 1;
    public static int[] lastClashCharacters = new int[] {1, 1, 1, 1};
    
    public static bool bossEncounter = true;
    
    public static arenaMode mode = arenaMode.Normal;
    public static arenaTheme theme = arenaTheme.Humanoids;
    
    public static int playersCount = 4;
    public static string[] playerNames = new string[] 
        {
            "Player A",
            "Player B", 
            "Player C",
            "Player D"
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
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            }
        );
        
    // Functions to manipulate things using tags
    
    public static GameObject getByName( string name)
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
    
    public static GameObject[] getByTag( string tag )
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }
    
    public static void setActiveByTag( string tag, bool active )
    {
        GameObject[] stuff = global.getByTag(tag);
        for (int i = stuff.Length - 1; i >= 0; i--)
            stuff[i].SetActive(active);
    }
    
    public static void setRandomlyActiveByTag( string tag )
    {
        GameObject[] stuff = global.getByTag(tag);
        for (int i = stuff.Length - 1; i >= 0; i--)
            stuff[i].SetActive(Random.value > 0.5f);
    }
    
    // Image functions
    
    public static void setImage( GameObject imageObject, string image )
    {
        if (imageObject == null) return;
            
        imageObject.GetComponent<Image>().sprite = 
            Resources.Load(image, typeof(Sprite)) as Sprite;
            
        Debug.Log(imageObject.name + " replaced with " + image);
    }
    
    // Audio functions
    
    public static void playClipAt( AudioClip clip, Vector3 where, float volume )
    {
        AudioSource.PlayClipAtPoint(clip, where, volume * audioVolume);
    }
    
    // Game control functions
    
    public static void restart()
    {
        Debug.Log("Reseting the game");
        
        clashMode = false;
        clashRounds = 5;
        clashRoundsPlayed = 0;
        clashVictories[0] = clashVictories[1] = 
                clashVictories[2] = clashVictories[3] = 0;
        
        bossEncounter = false;
        
        mode = arenaMode.Normal;
        theme = arenaTheme.Chess;
        
        playersCount = 4;
    }
    
    public static arenaMode randomArenaMode()
    {
        int which = Random.Range(0, (allowedArenaModes.Count - 1));
        return (arenaMode)(allowedArenaModes[which]);
    }
    
    public static void goToMainMenu()
    {
        Debug.Log("Main Menu");
        bossEncounter = false;
        SceneManager.LoadScene("mainMenu");
    }
    
    public static void loadProperArenaScene()
    {
        string scene = "mainMenu";
        mode = randomArenaMode();
        
        if (theme == arenaTheme.Cars) scene = "cars";
        else if (theme == arenaTheme.Humanoids) scene = "humanoids";
        else if (theme == arenaTheme.Fantasy) scene = "fantasy";
        else if (theme == arenaTheme.Chess)
        {
            scene = "chess";
            if (bossEncounter)
            {
                playersCount = 2;
                mode = arenaMode.Normal;
            }
        }
        else if (theme == arenaTheme.Abstract) scene = "abstract";
        else if (theme == arenaTheme.Secret) scene = "secret";
        
        SceneManager.LoadScene(scene);
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
