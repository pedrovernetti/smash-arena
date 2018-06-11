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
        Unstable = 6,   // arena can tilt and fall
        Meteors = 7,    // stones fall randomly on the arena
        Teleport = 8,   // teleporter objects
        Ghost = 9,      // characters almost invisible and randomly non-solid
        Funny = 10,     // messed physics, messed textures/colors, messed music
        Shrunken = 11   // characters are half their size
    };  
    
    public enum arenaTheme : byte
    {
        Cars = 1,       // characters are cars
        Humanoids = 2,  // characters are humanoid warriors/soldiers
        Fantasy = 4,    // characters are fantasy mobile game monsters
        Chess = 8,      // characters are chess pieces
        Abstract = 16,  // characters are basic 3d geometric forms
        Secret = 32     // characters are wizard and bizarre creatures
    };
    
    public enum difficultyLevel : byte
    {
        Easy = 0,
        Normal = 1,
        Hard = 2,
        Hell = 4
    };
    
    public enum gameResult : byte
    {
        WIN = 1,
        DRAW = 2,
        LOSE = 4
    }

    // Configurações
    
    public static float audioVolume = 1;
    public static float musicVolume = 0;
    public static bool fullscreen = false;
    
    // Jogo
    
    public static difficultyLevel difficulty = difficultyLevel.Normal;
    
    public static bool ongoingGame = false;
    
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
    
    public static bool bossEncounter = true;
    
    public static arenaMode mode = arenaMode.Ghost;
    public static arenaTheme theme = arenaTheme.Chess;
    
    public static int playersCount = 4;
    public static string[] playerNames = new string[] 
        {
            "Player A",
            "Player B", 
            "Player C",
            "Player D"
        };
        
    // Funções para manipulação baseada em tags
    
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
    
    // Funções de áudio
    
    public static void playClipAt( AudioClip clip, Vector3 where, float volume )
    {
        AudioSource.PlayClipAtPoint(clip, where, volume * audioVolume);
    }
    
    // Funções de controle do jogo
    
    public static void restart()
    {
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
    
    public static void loadProperArenaScene()
    {
        string scene = "newMenu";
        if (difficulty != difficultyLevel.Hell)
            mode = (arenaMode)(Random.Range(1, 5));
        else 
            mode = (arenaMode)(Random.Range(1, 7));
        if (theme == arenaTheme.Cars) scene = "carsScene";
        else if (theme == arenaTheme.Humanoids) scene = "humanoids";
        else if (theme == arenaTheme.Fantasy) scene = "fantasy";
        else if (theme == arenaTheme.Chess)
        {
            scene = "chessScene";
            if (bossEncounter)
            {
                playersCount = 2;
                mode = arenaMode.Normal;
            }
        }
        SceneManager.LoadScene(scene);
    }
    
    public static void advanceCampaign()
    {
        if (theme == arenaTheme.Chess)
        {
            if (!bossEncounter) bossEncounter = true;
            else 
            {
                restart();
                // zerou o jogo... chamar cutscene final.. mais alguma coisa
                return;
            }
        }
        else theme++;
        loadProperArenaScene(); // no lugar disso, chamar cutscene baseada no 
                                // tema recém settado, que aí sim chamará isso
    }
}
