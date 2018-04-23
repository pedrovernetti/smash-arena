using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneOnClick : MonoBehaviour {

	public void loabByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
