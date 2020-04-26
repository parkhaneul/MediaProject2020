using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject testUnit;
    public Dictionary<int, GameObject> unitList;

    private Camera _camera;
    private List<InputObservableController> controllers;
    public Vector3 playerModelScale = Vector3.one;
    
    void OnEnable()
    {
        if(controllers == null)
            controllers = new List<InputObservableController>();

        if(unitList == null)
            unitList = new Dictionary<int, GameObject>();
        
        if(_camera == null)
            _camera = Camera.main;
        
        newController(1234);
    }

    private void OnDisable()
    {
        foreach (var controller in controllers)
        {
            controller.moveEvent -= onMoveEvent;
            controller.actionEvent -= onActionEvent;
        }
        controllers = null;
    }

    void newTestUnit(int uid)
    {
        var go = GameObject.Instantiate(testUnit);
        go.gameObject.transform.localScale = playerModelScale;
        go.SetActive(true);

        this.UpdateAsObservable()
            .Select(_ => _camera.WorldToViewportPoint(go.transform.position))
            .Where(_ => (_.x < 1 && _.x > 0 && _.y < 1 && _.y > 0) == false)
            .DistinctUntilChanged(_ => _)
            .Subscribe(_ =>
            {
                UIManager.Instance.setActive(true);
                UIManager.Instance.test(_);
            })
            .AddTo(go);
        
        this.UpdateAsObservable()
            .Select(_ => _camera.WorldToViewportPoint(go.transform.position))
            .Where(_ => (_.x < 1 && _.x > 0 && _.y < 1 && _.y > 0))
            .DistinctUntilChanged(_ => _)
            .Subscribe(_ =>
            {
                UIManager.Instance.setActive(false);
            })
            .AddTo(go);
        
        unitList.Add(uid,go);
    }

    public void newController(int uid)
    {
        var ic = new InputObservableController(uid,this.gameObject);

        ic.moveEvent += onMoveEvent;
        ic.actionEvent += onActionEvent;
        
        newTestUnit(uid);
        addController(ic);
    }

    void addController(InputObservableController ic)
    {
        controllers.Add(ic);
    }

    public void onMoveEvent(object sender, KeyEventArgs<Point> e)
    {
        if (unitList.ContainsKey(e.uid) == false)
        {
            Logger.Log(e.uid + " unit lost");
        }
        else
        {
            GameObject unit;
            unitList.TryGetValue(e.uid,out unit);
            unit.GetComponent<CharacterAction>().move(e.value);
        }
    }

    public void onActionEvent(object sender, KeyEventArgs<Boolean> e)
    {
        if (unitList.ContainsKey(e.uid) == false)
        {
            Logger.Log(e.uid + " unit lost");
        }
        else
        {
            GameObject unit;
            unitList.TryGetValue(e.uid,out unit);
            unit.GetComponent<CharacterAction>().action(e.value);
        }
    }
}
