using System;
﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class selectionImage : MonoBehaviour
{
    public enum imageType : byte
    {
        theme = 4,
        mode = 5,
        player1 = 0,
        player2 = 1,
        player3 = 2,
        player4 = 3
    };
    
    private int which;
    private int internalIndex;
    private GameObject[] possibilities;
    
    public imageType type;
        
    public void Start()
    {
        which = 0;        
        internalIndex = 0;     
        possibilities = new GameObject[transform.childCount];
        for (int i = possibilities.Length - 1; i >= 0; i--)
        {
            possibilities[i] = transform.GetChild(i).gameObject;
            if (i != 0) possibilities[i].SetActive(false);
            else possibilities[i].SetActive(true);
        }
    }
	
    public void FixedUpdate()
    {
        if ((type == imageType.theme) && (which != UIController.whichTheme))
        {
            possibilities[internalIndex].SetActive(false);
            which = UIController.whichTheme;
            internalIndex = (which == 0) ? 0 : (int)(global.theme);
            possibilities[internalIndex].SetActive(true);
        }
        else if ((type == imageType.mode) && (which != UIController.whichMode))
        {
            possibilities[internalIndex].SetActive(false);
            which = UIController.whichMode;
            internalIndex = (which == 0) ? 0 : (int)(global.mode);
            possibilities[internalIndex].SetActive(true);
        }
        else if ((type >= imageType.player1) && (type <= imageType.player4) &&
                 (possibilities[0].name != global.playerCharacters[(int)(type)]))
        {
            possibilities[0].SetActive(false);
            GameObject current = 
                global.getChildByName(gameObject, global.playerCharacters[(int)(type)]);
            if (current != null) 
            {
                current.SetActive(true);
                possibilities[0] = current;
            }
            else
            {
                current = global.getChildByName(gameObject, "missing");
                if (current != null) 
                {
                    current.SetActive(true);
                    possibilities[0] = current;
                }
            }
        }
	} 
}
