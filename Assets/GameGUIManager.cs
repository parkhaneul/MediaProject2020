using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameGUIManager : MonoBehaviour
{
    public UnityEvent AddPlayerButtonAction;
    
    public void OnGUI()
    {
        GUI.Box(new Rect(10,10,100,90),"Test GUI");

        if (GUI.Button(new Rect(20, 40, 80, 20), "Add Player"))
        {
            AddPlayerButtonAction.Invoke();
        }
    }
}
