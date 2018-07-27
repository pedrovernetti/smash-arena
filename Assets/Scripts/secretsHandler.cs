using System;
﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class secretsHandler
{
    private static string secretCode = "";
    private static DateTime lastSecretInput; 
    private static DateTime lastSecretInputRead; 
    
    private static int timeLimit = 1000 / (int)(global.difficulty);
    
    private static bool waitingSecondCode = false;
    
    public static string code
    {
        get { return secretCode; }
        set { secretCode = value; }
    }
    
    // Recarrega a arena atual
    public static void editorTrick1()
    {
        if (global.cheated) SceneManager.LoadScene(global.currentScene);
    }
    
    // Recarrega a arena atual num modo aleatório
    public static void secretTrick1()
    {
        global.bossEncounter = false;
        global.loadProperArenaScene();
    }
    
    public static void secretTrick2( string secondCode = "" )
    {
        if (waitingSecondCode)
        {
            waitingSecondCode = false;
            int codeValue = Int32.Parse(secondCode);
            if ((codeValue > 0) && (codeValue < 15))
            {
                global.mode = (global.arenaMode)(codeValue);
                global.loadProperArenaScene();
            }
        }
        else waitingSecondCode = true;
    }
    
    public static void secretTrick3()
    {
    }
    
    // Libera tudo
    public static void secretTrick4()
    {
        global.allowedArenaModes = new ArrayList(new global.arenaMode[]
            { global.arenaMode.Normal, global.arenaMode.Inverted, 
              global.arenaMode.Frozen, global.arenaMode.Burning,
              global.arenaMode.Electric, global.arenaMode.Unstable,
              global.arenaMode.Shrunken, global.arenaMode.Enlarged,
              global.arenaMode.Dark, global.arenaMode.DownsideUp });
            
        global.allowedArenaThemes = new ArrayList(new global.arenaTheme[]
            { global.arenaTheme.Cars, global.arenaTheme.Humanoids, 
              global.arenaTheme.Fantasy, global.arenaTheme.Chess });
        
        global.allowedCharacters = new ArrayList(new string
            [ global.chessEnemies.Length + global.humanoidEnemies.Length +
              global.carEnemies.Length + global.fantasyEnemies.Length ]);
        int j = 0;
        for (int i = global.chessEnemies.Length - 1; i >= 0; i--, j++)
            global.allowedCharacters[j] = global.chessEnemies[i].Item1;
        for (int i = global.fantasyEnemies.Length - 1; i >= 0; i--, j++)
            global.allowedCharacters[j] = global.fantasyEnemies[i].Item1;
        for (int i = global.humanoidEnemies.Length - 1; i >= 0; i--, j++)
            global.allowedCharacters[j] = global.humanoidEnemies[i].Item1;
        for (int i = global.carEnemies.Length - 1; i >= 0; i--, j++)
            global.allowedCharacters[j] = global.carEnemies[i].Item1;
            
        global.cheated = true;
        global.currentArena.showText("Everything is now unlocked", 2f, true);
    }
    
    private static void secretTrick5()
    {
        global.playerCharacters =  new string[] 
            { "missingNo1", "missingNo2", "missingNo3", "missingNo4" };
        global.currentArena.showText("WTF???", 2f, true);
        
        Transform[] players = 
            GameObject.Find("Players").GetComponentsInChildren<Transform>(true);
        foreach (Transform player in players)
        {
            if (player.gameObject.CompareTag("Player") && 
               (player.gameObject.GetComponent<playerController>() != null))
            {
                player.gameObject.GetComponent<playerController>().Awake();
                if (player.gameObject.activeInHierarchy)
                    player.gameObject.GetComponent<playerController>().Start();
            }
        }
    }

    public static void readSecretCode()
    {
        if ((global.now - lastSecretInput).TotalMilliseconds > timeLimit)
            secretCode = "";
        if ((global.now - lastSecretInputRead).TotalMilliseconds > 100) 
        {
            int formerLength = secretCode.Length;
            lastSecretInputRead = global.now;
            
            if (Input.GetKey(KeyCode.Alpha0)) secretCode += '0';
            else if (Input.GetKey(KeyCode.Alpha1)) secretCode += '1';
            else if (Input.GetKey(KeyCode.Alpha2)) secretCode += '2';
            else if (Input.GetKey(KeyCode.Alpha3)) secretCode += '3';
            else if (Input.GetKey(KeyCode.Alpha4)) secretCode += '4';
            else if (Input.GetKey(KeyCode.Alpha5)) secretCode += '5';
            else if (Input.GetKey(KeyCode.Alpha6)) secretCode += '6';
            else if (Input.GetKey(KeyCode.Alpha7)) secretCode += '7';
            else if (Input.GetKey(KeyCode.Alpha8)) secretCode += '8';
            else if (Input.GetKey(KeyCode.Alpha9)) secretCode += '9';
            else if (Input.GetKey(KeyCode.UpArrow)) secretCode += 'U';
            else if (Input.GetKey(KeyCode.DownArrow)) secretCode += 'D';
            else if (Input.GetKey(KeyCode.LeftArrow)) secretCode += 'L';
            else if (Input.GetKey(KeyCode.RightArrow)) secretCode += 'R';
            
            if (secretCode.Length > formerLength) 
            {
                lastSecretInput = lastSecretInputRead;
                Debug.Log("secret: " + secretCode);
            }
            
            if (waitingSecondCode) secretTrick2(secretCode);            
            #if UNITY_EDITOR
            if (secretCode.EndsWith("11")) editorTrick1();
            #else
            if (secretCode.EndsWith("11DD")) editorTrick1();
            #endif
            if (secretCode.EndsWith("RULDL")) secretTrick1();
            if (secretCode.EndsWith("LLUU")) secretTrick2();
            if (secretCode.EndsWith("UUDDLRLR12")) secretTrick3();
            if (secretCode.EndsWith("1RDL")) secretTrick4();
            if (secretCode.EndsWith("6655321")) secretTrick5();
        }
    }
}
