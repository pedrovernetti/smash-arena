using System;
﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class secretsHandler
{
    private static string secretCode = "";
    private static int superCodeCount = 0;
    private static DateTime lastSecretInput; 
    private static DateTime lastSecretInputRead; 
    
    private static int timeLimit = 1000 / (int)(global.difficulty);
    
    public static string code
    {
        get { return secretCode; }
        set { secretCode = value; }
    }
    
    public static void reload()
    {
        if (global.cheated) SceneManager.LoadScene(global.currentScene);
    }
    
    public static void reloadWithRandomMode()
    {
        global.bossEncounter = false;
        global.loadProperArenaScene();
    }
    
    public static void unlockBossForClash()
    {
        if (global.allowedCharacters.Contains("boss")) return;
        
        global.allowedCharacters.Add("boss");
        global.currentArena.showText(
            "Now you are become Death,\nthe destroyer of worlds...", 2f, true);
    }
    
    public static void specialTrick()
    {
        if (superCodeCount == 0)
            global.currentArena.showText("What do you expect?", 2f, true);
        else if (superCodeCount == 1)
            global.currentArena.showText("...", 2f, true);
    }
    
    public static void unlockEverything()
    {
        if (global.cheated) return;
        
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
    
    // Libera MissingNo.
    private static void fightWithMissingNo()
    {
        global.playerCharacters =  new string[] 
            { "missingNo1", "missingNo2", "missingNo3", "missingNo4" };
        global.currentArena.showText("WTF???", 2f, true);
        
        Transform[] players = 
            GameObject.Find("players").GetComponentsInChildren<Transform>(true);
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
    
    private static void getInput()
    {
        if (Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.JoystickButton0)) 
            secretCode += '0';
        else if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.JoystickButton1))
            secretCode += '1';
        else if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton2))
            secretCode += '2';
        else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.JoystickButton3)) 
            secretCode += '3';
        else if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.JoystickButton4)) 
            secretCode += '4';            
        else if (Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.JoystickButton5)) 
            secretCode += '5';
            
        else if (Input.GetKey(KeyCode.Alpha6)) secretCode += '6';
        else if (Input.GetKey(KeyCode.Alpha7)) secretCode += '7';
        else if (Input.GetKey(KeyCode.Alpha8)) secretCode += '8';
        else if (Input.GetKey(KeyCode.Alpha9)) secretCode += '9';
        
        else if (global.verticalInput() > 0.0f) secretCode += 'U';
        else if (global.verticalInput() < 0.0f) secretCode += 'D';
        else if (global.horizontalInput() > 0.0f) secretCode += 'R';
        else if (global.horizontalInput() < 0.0f) secretCode += 'L';
    }

    public static void readSecretCode()
    {
        if ((global.now - lastSecretInput).TotalMilliseconds > timeLimit)
            secretCode = "";
        if ((global.now - lastSecretInputRead).TotalMilliseconds > 100) 
        {
            int formerLength = secretCode.Length;
            lastSecretInputRead = global.now;
            
            getInput();
            
            if (secretCode.Length > formerLength) 
            {
                lastSecretInput = lastSecretInputRead;
                Debug.Log("secret: " + secretCode);
            }
            
            if (secretCode.EndsWith("UUDDLRLR21")) specialTrick();
            else if (superCodeCount > 0) global.currentArena.showText(":(", 2f, true);
            else superCodeCount = 0;            
            if (secretCode.EndsWith("11")) reload();
            if (secretCode.EndsWith("RULDL")) reloadWithRandomMode();
            if (secretCode.EndsWith("28064212")) unlockBossForClash();
            if (secretCode.EndsWith("1RDL")) unlockEverything();
            if (secretCode.EndsWith("6655321")) fightWithMissingNo();
        }
    }
}
