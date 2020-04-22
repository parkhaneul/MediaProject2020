using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBarObject : MonoBehaviour
{
    private MissionLogic mLogic;

    public void Start()
    {
        if (mLogic == null)
            mLogic = MissionLogic.Instance;
    }
}
