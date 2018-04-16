using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarScript : MonoBehaviour {

	Image EnergyBar;
	float maxenergy = 100f;
	public static float energy;
	// Use this for initialization
	void Start () {
		EnergyBar = GetComponent<Image> ();
		energy = maxenergy;
	}
	
	// Update is called once per frame
	void Update () {
		EnergyBar.fillAmount = energy / maxenergy;
	}
}
