﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameGUIManager : MonoBehaviour
{
    public bool testGUI = true;
    
    public UnityEvent AddPlayerButtonAction;
    public UnityEvent RespawnItemButtonAction;
    public UnityEvent TimeResetButtonAction;
    public void OnGUI()
    {
        if (testGUI == false)
            return;
        
        GUI.Box(new Rect(10,10,150,130),"Test GUI");

        if (GUI.Button(new Rect(20, 40, 130, 20), "Add Player"))
        {
            AddPlayerButtonAction.Invoke();
        }

        if (GUI.Button(new Rect(20, 70, 130, 20), "Respawn Item"))
        {
            RespawnItemButtonAction.Invoke();
        }

        if (GUI.Button(new Rect(20, 100, 130, 20), "Time Reset"))
        {
            TimeResetButtonAction.Invoke();
        }
    }
}
