using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface GameLogic
{
    void active();
    void stop();
    void run();
    void mainLogic();
}

public class BasicLogic<T> : GameLogic where T : class, new()
{
    public static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }
    
    private bool _flag = false;
    
    public void active()
    {
        if(_flag == false)
            _flag = true;
    }

    public void stop()
    {
        if (_flag)
            _flag = false;
    }

    public void run()
    {
        if(_flag)
            mainLogic();
    }

    public virtual void mainLogic()
    {
    }
}

public class TimeLogic : BasicLogic<TimeLogic>
{
    private float _limitTime; //제한 시간
    private float _currentTime; //제한 시간 중 남은 시간

    private const float _zeroTime = 0f;

    public TimeLogic()
    {
        Logger.LogWarning("Time Logic Running...");
    }

    public void setTime(float time)
    {
        _limitTime = time;
        _currentTime = time;
    }

    public void countDownTik()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime < _zeroTime)
        {
            Logger.LogError("TimeOver");
            stop();
        }
    }

    public override void mainLogic()
    {
        countDownTik();
    }

    public void addTime(float time)
    {
        _currentTime += time;
        if (_currentTime > _limitTime)
            _currentTime = _limitTime;
    }
}

/// <summary>
/// change code if Item Data Class Update
/// </summary>
public class MissionLogic : BasicLogic<MissionLogic>
{
    private float missionPercent = 0;
    private float tempPercent = 0;
    private const float _interval = 0.5f;
    
    private List<string> missionItemList = new List<string>();
    public MissionLogic()
    {
        Logger.LogWarning("Mission Logic Running");
    }

    //item class 정해지면 변경
    public void setList(List<string> list)
    {
        missionItemList = list;
    }
    
    //item class 정해지면 변경
    public bool checkItemRequired(string itemName)
    {
        var returnValue = false;

        foreach (var name in missionItemList)
        {
            returnValue |= (itemName == name);
        }

        return returnValue;
    }

    public override void mainLogic()
    {
        if (tempPercent < missionPercent)
        {
            tempPercent += _interval;
        }
    }

    public void addPercent(float value)
    {
        if (missionPercent + value < 100)
        {
            missionPercent += value;
        }
        else
        {
            missionPercent = 100;
        }
    }

    public float getPercent()
    {
        return tempPercent;
    }
}