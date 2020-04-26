using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CameraScript : MonoBehaviour
{
    public GameObject TraceObject;
    public Vector3 DistanceVector;
    
    public void Start()
    {
        this.ObserveEveryValueChanged(_ => _.TraceObject.transform.position)
            .Subscribe(_ => trace())
            .AddTo(this);
    }

    public void trace()
    {
        this.transform.position = TraceObject.transform.position + DistanceVector;
    }
}
