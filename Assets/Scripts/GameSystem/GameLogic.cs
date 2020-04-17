using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _limitTime; //제한 시간
    private float _currentTime; //제한 시간 중 남은 시간

    private const float _zeroTime = 0f;
    
    public TimeLogic(float time)
    {
        _limitTime = time;
        _currentTime = time;
        
        Logger.LogWarning("Time Logic Running : " + time + " seconds");
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
    
    public override void run()
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

public class MissionLogic : BasicLogic
{
    public MissionLogic()
    {
        Logger.LogWarning("Mission Logic Running");
    }
    
    public override void run()
    {
    }
}

public class User
{
    public int uid;

    public string xAxisInput;
    public string yAxisInput;
    public string ActionInput;
}

public class GameConnectionLogic : BasicLogic
{
    private List<User> users;
    
    public GameConnectionLogic()
    {
        if (users == null)
            users = new List<User>();
    }
    public override void run()
    {
    }
}