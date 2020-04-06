using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject testUnit;
    public Dictionary<int, GameObject> unitList;
    private List<InputController> controllers;

    public void Update()
    {
        if (controllers == null)
            return;

        foreach (var controller in controllers)
        {
            controller.keyInput();
        }
    }

    void OnEnable()
    {
        if(controllers == null)
            controllers = new List<InputController>();

        if(unitList == null)
            unitList = new Dictionary<int, GameObject>();
        
        newController();
        
        foreach (var controller in controllers)
        {
            controller.moveEvent += onMoveEvent;
            controller.actionEvent += onActionEvent;
        }
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

    void newController()
    {
        var uid = 1234;
        var ic = new InputController(uid);
        newTestUnit(uid);
        addController(ic);
    }

    void addController(InputController ic)
    {
        controllers.Add(ic);
    }

    public void onMoveEvent(object sender, KeyEventArgs<Point> e)
    {
        if (unitList.ContainsKey(e.uid) == false)
        {
            Debug.Log(e.uid + " unit lost");
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
        Debug.Log(e.uid + " action : " + e.value);
    }
}
