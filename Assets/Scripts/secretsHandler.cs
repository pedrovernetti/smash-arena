using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class secretsHandler
{
    private static string secretCode = "";
    private static DateTime lastSecretInput; 
    private static DateTime lastSecretInputRead; 
    
    private static int timeLimit = 1000 / (int)(global.difficulty);
    
    public static string code
    {
        get { return secretCode; }
        set { secretCode = value; }
    }
    
    // Recarrega a arena atual
    // BEGIN : GAMBIARRA
    public static int editorTrickUseCounter = 0;
    public static void editorTrick1( bool avoidRecursion = false )
    {
        editorTrickUseCounter++;
        if (avoidRecursion && (editorTrickUseCounter > 1)) return;
        SceneManager.LoadScene(global.currentScene);
    }
    // END : GAMBIARRA
    
    // Recarrega a arena atual num modo aleatório
    private static void secretTrick1()
    {
        global.bossEncounter = false;
        global.loadProperArenaScene();
    }
    
    private static void secretTrick2()
    {
    }
    
    private static void secretTrick3()
    {
    }
    
    private static void secretTrick4()
    {
    }
    
    private static void secretTrick5()
    {
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
               
            
            #if UNITY_EDITOR
            if (secretCode.EndsWith("11")) editorTrick1();
            #endif
            if (secretCode.EndsWith("RULDL")) secretTrick1();
            if (secretCode.EndsWith("LLUU")) secretTrick2();
            if (secretCode.EndsWith("UUDDLRLR12")) secretTrick3();
            if (secretCode.EndsWith("0112358")) secretTrick4();
            if (secretCode.EndsWith("6655321")) secretTrick5();
        }
    }
}
