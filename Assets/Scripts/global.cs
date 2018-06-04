using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class global 
{
    public enum arenaMode : byte
    {
        Normal = 1,
        Inverted = 2,
        Frozen = 3,
        Burning = 4,
        Electric = 5
    };  
    
    public enum arenaTheme : byte
    {
        Cars = 1,
        Humanoids = 2,
        Fantasy = 4,
        Chess = 8
    };
    
    public enum gameResult : byte
    {
        WIN = 1,
        DRAW = 2,
        LOSE = 4
    }
    
    public enum activeness : byte
    {
        False = 0,
        True = 1,
        Random = 2
    };

    // Configurações
    
    public static float audioVolume = 1;
    public static float musicVolume = 0;
    public static bool fullscreen = false;
    
    // Jogo
    
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
    
    public static bool bossEncounter = false;
    
    public static arenaMode mode = arenaMode.Inverted;
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
    
    public static void setActiveByTag( string tag, activeness active )
    {
        GameObject[] stuff = global.getByTag(tag);
        bool active_bool = ((int)(active) != 0);
        for (int i = stuff.Length - 1; i >= 0; i--)
        {
            if (!(active == activeness.Random)) stuff[i].SetActive(active_bool);
            else stuff[i].SetActive(Random.value > 0.5f);
        }
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
        mode = (arenaMode)(Random.Range(1, 5));
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
