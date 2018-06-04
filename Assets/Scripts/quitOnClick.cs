using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitOnClick : MonoBehaviour
{
    public void quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
        #else
        Application.Quit();
        #endif
    }
}
