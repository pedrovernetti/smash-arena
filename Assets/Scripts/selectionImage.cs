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
    
    public void findWhichByCharacterName()
    {
        which = possibilities.Length;
        for (int i = possibilities.Length - 1; i >= 0; i--)
        {
            if (global.playerCharacters[(int)(type)] == possibilities[i].name) 
                which = i;
        }
        if (which != possibilities.Length) return;
        for (int i = possibilities.Length - 1; i >= 0; i--)
        {
            if (global.playerCharacters[(int)(type)].StartsWith(possibilities[i].name)) 
                which = i;
        }
    }
    
    private void tryNewInternalIndex()
    {
        if (internalIndex < possibilities.Length) 
            possibilities[internalIndex].SetActive(true);
        else internalIndex = which = 0;
    }
    
    public void Awake()
    {
        possibilities = new GameObject[transform.childCount];
        for (int i = possibilities.Length - 1; i >= 0; i--)
        {
            possibilities[i] = transform.GetChild(i).gameObject;
            possibilities[i].SetActive(false);
        }   
        
        if (type > imageType.player4) which = 0;
        else findWhichByCharacterName();
        internalIndex = which;
        
        tryNewInternalIndex();
    }
	
    public void Update()
    {
        if ((type == imageType.theme) && (which != UIController.whichTheme))
        {
            possibilities[internalIndex].SetActive(false);
            which = UIController.whichTheme;
            internalIndex = (which == 0) ? 0 : (int)(global.theme);
            tryNewInternalIndex();
        }
        else if ((type == imageType.mode) && (which != UIController.whichMode))
        {
            possibilities[internalIndex].SetActive(false);
            which = UIController.whichMode;
            internalIndex = (which == 0) ? 0 : (int)(global.mode);
            tryNewInternalIndex();
        }
        else if ((type >= imageType.player1) && (type <= imageType.player4) &&
                 (possibilities[0].name != global.playerCharacters[(int)(type)]))
        {
            possibilities[internalIndex].SetActive(false);
            findWhichByCharacterName();
            internalIndex = which;
            tryNewInternalIndex();
        }
	} 
}
