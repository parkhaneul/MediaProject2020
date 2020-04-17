using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject testUnit;
    public Dictionary<int, GameObject> unitList;
    
    private List<InputObservableController> controllers;
    
    void OnEnable()
    {
        if(controllers == null)
            controllers = new List<InputObservableController>();

        if(unitList == null)
            unitList = new Dictionary<int, GameObject>();
        
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
        var unit = GameObject.Instantiate(testUnit);
        unit.SetActive(true);
        unitList.Add(uid,unit);
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
