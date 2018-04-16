using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void newGameBtn(string newGame)
	{
		SceneManager.LoadScene (newGame);
	}
}
