using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public float LimitTime = 0f;
    
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

    public List<BasicLogic> logics = new List<BasicLogic>();

    public void Awake()
    {
        logics.Add(new TimeLogic(LimitTime));
        
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
            if(logic.flag)
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
}

public interface GameLogic
{
    void active();
    void stop();
    void run();
}

public abstract class BasicLogic : GameLogic
{
    public bool flag = false;
    
    public void active()
    {
        if(flag == false)
            flag = true;
    }

    public void stop()
    {
        if (flag)
            flag = false;
    }

    public abstract void run();
}

public class TimeLogic : BasicLogic
{
    private float LimitTime; //제한 시간
    private float currentTime; //제한 시간 중 남은 시간
    
    public TimeLogic(float time)
    {
        LimitTime = time;
        currentTime = time;
    }

    public void countDownTik()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
        {
            Debug.Log("TimeOver");
            stop();
        }
    }
    
    public override void run()
    {
        countDownTik();
    }

    public void addTime(float time)
    {
        currentTime += time;
        if (currentTime > LimitTime)
            currentTime = LimitTime;
    }
}