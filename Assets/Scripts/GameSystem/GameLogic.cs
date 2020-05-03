using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public interface GameLogic
{
    void active();
    void stop();
    void run();
    void mainLogic();
}

public class BasicLogic<T> : GameLogic where T : class, new()
{
    private static T _instance;
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

/// <summary>
/// object polling class
/// </summary>
public class ObjectRecyclingLogic : BasicLogic<ObjectRecyclingLogic>
{
    private Dictionary<string, GameObject> trashCan;

    public ObjectRecyclingLogic()
    {
        if(trashCan == null)
            trashCan = new Dictionary<string, GameObject>();
    }

    public void chunk(string name, GameObject go)
    {
        trashCan[name] = go;
        go.SetActive(false);
        
        Logger.Log("TrashCan has " + trashCan.Count + " Items");
    }

    [CanBeNull]
    public GameObject randomPickUp()
    {
        if (trashCan.Count == 0)
            return null;

        var rValue = UnityEngine.Random.Range(0, trashCan.Count);

        return pickUp(trashCan.Keys.ToArray()[rValue]);
    }

    [CanBeNull]
    public GameObject pickUp(string name)
    {
        if (trashCan.ContainsKey(name))
        {
            var go = trashCan[name];
            trashCan.Remove(name);
            go.SetActive(true);
            return go;
        }

        Logger.Log(name + " is not in trashCan.");
        
        return null;
    }
    
    public override void mainLogic()
    {
    }
}

public class TimeLogic : BasicLogic<TimeLogic>
{
    private float _limitTime; //제한 시간
    private float _currentTime; //제한 시간 중 남은 시간

    private Text testText;
    private const float _zeroTime = 0f;

    public TimeLogic()
    {
        Logger.LogWarning("Time Logic Running...");
    }

    public void setText(Text text)
    {
        testText = text;
    }
    
    public void setTime(float time)
    {
        _limitTime = time;
        _currentTime = time;
    }

    public void countDownTik()
    {
        _currentTime -= Time.deltaTime;

        testText.text = _currentTime.ToString();
        
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
    private float missionPercent = 0;
    private float animationPurposePercent = 0;
    private float _interval = 0.5f;
    
    private RequiredItems missionItemList = new RequiredItems();
    private RequiredItems possessingItems = new RequiredItems();
    public MissionLogic()
    {
        Logger.LogWarning("Mission Logic Running");
    }

    public void setList(RequiredItems list)
    {
        missionItemList = list;
        possessingItems = new RequiredItems();
        possessingItems.requiredItems = new Dictionary<ItemKind, int>();
    }

    public void setInterval(float value)
    {
        _interval = value;
    }
    
    public bool isItemRequired(ItemKind itemName)
    {
        bool isTarget = missionItemList.requiredItems.Any( i => i.Key == itemName) ;
        bool isNeededMore = false;
        int required = 0, possess = 0;
        if(missionItemList.requiredItems.TryGetValue(itemName, out required))
        {
            possessingItems.requiredItems.TryGetValue(itemName, out possess);
            isNeededMore = required > possess;
        }
        return isTarget && isNeededMore;
    }

    public override void mainLogic()
    {
        if (animationPurposePercent < missionPercent)
        {
            animationPurposePercent += _interval;
        }
    }
    public void putItem(Item item)
    { 
        if(missionItemList.requiredItems.ContainsKey(item.kind))
        {
            if(possessingItems.requiredItems.ContainsKey(item.kind))
            {
                if(possessingItems.requiredItems[item.kind] <
                    missionItemList.requiredItems[item.kind])
                {
                    possessingItems.requiredItems[item.kind] = possessingItems.requiredItems[item.kind] + 1;   
                }
                else
                {
                    Debug.LogError("You can't put item more than requied amount.");
                    return;
                }
            }
            else
            {
                possessingItems.requiredItems.Add(item.kind, 1);
            }
        }
        else
        {
            Debug.LogError("You can't put item to misson logic that is not required.");
            return;
        }
        updatePercent();
    }
    private void updatePercent()
    {
        int total = 0, current = 0;
        foreach(var item in missionItemList.requiredItems)
        {
            total += item.Value;
        }
        foreach(var item in possessingItems.requiredItems)
        {
            current += item.Value;
        }

        missionPercent = (float)total / current;
    }

    public float getPercent()
    {
        return animationPurposePercent;
    }
}

public class PlayerConnectionLogic : BasicLogic<PlayerConnectionLogic>
{
    private int _currentPlayerNumber;
    public int currentPlayerNumber
    {
        get { return _currentPlayerNumber; }
    }

    private int _maximumPlayerNumber;
    public int maximumPlayerNumber
    {
        get { return _maximumPlayerNumber; }
    }

    private Dictionary<int, int> deviceAndUser;

    public PlayerConnectionLogic()
    {
        if(deviceAndUser == null)
            deviceAndUser = new Dictionary<int, int>();
    }
    
    public bool addPlayer(int deviceID, int uid)
    {
        if (currentPlayerNumber < maximumPlayerNumber)
        {
            deviceAndUser[deviceID] = uid;
            _currentPlayerNumber++;
            return true;
        }
        else
            return false;
    }

    public int getUid(int deviceID)
    {
        if (deviceAndUser.ContainsKey(deviceID))
            return deviceAndUser[deviceID];

        return -1;
    }

    public void setMaximumNumber(int value)
    {
        _maximumPlayerNumber = value;
    }
    
    public override void mainLogic()
    {
    }
}