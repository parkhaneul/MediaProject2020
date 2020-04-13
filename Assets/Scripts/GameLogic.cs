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
        currentTime += time;
        if (currentTime > LimitTime)
            currentTime = LimitTime;
    }
}