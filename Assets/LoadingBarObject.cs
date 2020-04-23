using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.TestTools;

public class LoadingBarObject : MonoBehaviour
{
    private MissionLogic mLogic;
    private float percent;

    /// <summary>
    /// basicPivot is (0,0,0) ,(0,0,0) is (-1,-1,-1), (1,1,1) is (1,1,1)
    /// </summary>
    public Vector3 pivot = Vector3.zero;
    private Vector3 initPosition;
    //(-10,0,0)
    public float interval = 0.1f;
    public void Start()
    {
        initPosition = this.transform.position - pivot/2;
        
        if (mLogic == null)
            mLogic = MissionLogic.Instance;

        this.ObserveEveryValueChanged(_ => _.mLogic.getPercent())
            .Subscribe(_ =>
            {
                this.gameObject.transform.localScale = Vector3.one + Vector3.right * _ * interval;
                this.gameObject.transform.position = initPosition + mulitple(Vector3.right/2,gameObject.transform.localScale - Vector3.one);
            })
            .AddTo(this);
    }

    public Vector3 mulitple(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}