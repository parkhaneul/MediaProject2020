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

public class MissionLogic : BasicLogic<MissionLogic>
{
    public MissionLogic()
    {
        Logger.LogWarning("Mission Logic Running");
    }

    public override void mainLogic()
    {
    }
}