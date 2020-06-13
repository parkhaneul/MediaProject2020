using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    public Text TimeText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTimeText(float b)
    {
        string t = (int)b / 60+ ":" + (int)b % 60;
        TimeText.text = t;
    }
}
