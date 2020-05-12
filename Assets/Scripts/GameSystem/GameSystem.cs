using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public int maximumUserNumber = 4;
    public float LimitTime = 0f;
    public RequiredItems missionItemList = new RequiredItems();
    public float Interval;
    public Text text;
    
    private GameSystem _instance;
    public GameSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = this;
            return _instance;
        }
    }

    public List<GameLogic> logics = new List<GameLogic>();

    public void Awake()
    {
        var tl = TimeLogic.Instance;
        var ml = MissionLogic.Instance;
        var pcl = PlayerConnectionLogic.Instance;
        var orl = ObjectRecyclingLogic.Instance;
        var pmll = PlayerMoveLimitLogic.Instance;
        
        missionItemList = loadStage1Misson();

        tl.setTime(LimitTime);
        tl.setText(text);
        ml.setList(missionItemList);
        ml.setInterval(Interval);
        pcl.setMaximumNumber(maximumUserNumber);
        pmll.addBorder(new BorderCube().setBorder(-6,-2,-5,8,2,5).setMove(true));
        
        logics.Add(tl);
        logics.Add(ml);
        logics.Add(pcl);
        logics.Add(orl);
        logics.Add(pmll);
        
        activeAll();
    }

    public void Update()
    {
        runAll();
    }

    public void runAll()
    {
        foreach (var logic in logics)
        {
            logic.run();
        }
    }

    public void stopAll()
    {
        foreach (var logic in logics)
        {
            logic.stop();
        }
    }

    public void activeAll()
    {
        foreach (var logic in logics)
        {
            logic.active();
        }
    }

    public void randomPickUpInTrashCan()
    {
        var go = ObjectRecyclingLogic.Instance.randomPickUp();

        if (go == null)
            Logger.Log("TrashCan is Blank.");
        else
            go.transform.position = Vector3.up;
    }

    private RequiredItems loadStage1Misson()
    {  
        return new Stage1().GetRequiredItems();
    }
}