using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LoadingBarObject : MonoBehaviour
{
    private MissionLogic mLogic;
    private float percent;
    
    public void Start()
    {
        if (mLogic == null)
            mLogic = MissionLogic.Instance;

        this.ObserveEveryValueChanged(_ => _.mLogic.getPercent())
            .Subscribe(_ =>
            {
                this.gameObject.transform.position = Vector3.up * 0.5f + Vector3.right * (-9.5f + _ * 0.05f);
                this.gameObject.transform.localScale = Vector3.one + Vector3.right * _ * 0.1f;
            })
            .AddTo(this);
    }
}