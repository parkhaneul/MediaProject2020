using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class KeyEventArgs<T> : EventArgs
{
    public int uid;
    public T value;

    public KeyEventArgs()
    {
        value = default(T);
    }

    public KeyEventArgs(int uid, T value)
    {
        this.uid = uid;
        this.value = value;
    }
}

public class Point
{
    public float x;
    public float y;
    public float z;

    public Point(float x, float y,float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return "( " + x.ToString() + " : " + y.ToString() + " : " + z.ToString() + " )";
    }
}

public class InputController
{
    private int _uid;
    public event EventHandler<KeyEventArgs<Point>> moveEvent;
    public event EventHandler<KeyEventArgs<Boolean>> actionEvent;
    public event EventHandler<KeyEventArgs<Boolean>> noInputEvent;
    
    private Repeater _key = new Repeater(0.05f,0.025f,"Horizontal","","Vertical");
    private Repeater _action = new Repeater(0.2f,0.1f,"Fire1");
    
    public InputController(int uid)
    {
        this._uid = uid;
    }
    
    public void keyInput()
    {
        Point k = _key.getPoint();
        Boolean a = _action.getBoolean();

        if (k != null)
        {
            if (moveEvent != null)
            {
                moveEvent(this, new KeyEventArgs<Point>(_uid,k));
            }
        }

        if (a == true)
        {
            if (actionEvent != null)
            {
                actionEvent(this, new KeyEventArgs<bool>(_uid,a));
            }
        }

        if (k == null && a == false)
        {
            //noInputEvent(this,new KeyEventArgs<bool>(_uid,false));
        }
    }
}

public class Repeater
{
    private float threshold;
    private float rate;
    private float[] _next;
    private bool[] _hold;
    private string[] _axisArray;
    
    public Repeater(float threshold = 0.2f, float rate = 0.1f, params string[] axisNames)
    {
        this.threshold = threshold;
        this.rate = rate;
        _axisArray = axisNames;
        
        _next = new float[_axisArray.Length];
        _hold = new bool[_axisArray.Length];
    }

    public bool getBoolean()
    {
        float[] returnValue = tikTimeCheck(0);
        
        return returnValue[0] > 0;
    }

    public Point getPoint()
    {
        float[] returnValue = tikTimeCheck(0, 1, 2);
        float xValue = returnValue[0];
        float zValue = returnValue[2];

        if(xValue != 0 || zValue != 0)
            return new Point(xValue,0,zValue);

        return null;
    }

    /// <summary>
    /// 입력 Value는 Index임
    /// Index에 따라서 _next, _hold를 별도로 관리하기 위함임.
    /// </summary>
    /// <param name="values">_axisIndex</param>
    /// <returns></returns>
    public float[] tikTimeCheck(params int[] values)
    {
        if(values.Length != _axisArray.Length)
            Logger.LogError("Repeater Value Error");

        float[] returnValue = new float[_axisArray.Length]; 
        
        foreach (var value in values)
        {
            try
            {
                var getValue = Input.GetAxisRaw(_axisArray[value]);

                if (getValue != 0)
                {
                    if (Time.time > _next[value])
                    {
                        returnValue[value] = getValue;
                        _next[value] = Time.time + (_hold[value] ? rate : threshold);
                        _hold[value] = true;
                    }
                }
                else
                {
                    returnValue[value] = 0;
                    _hold[value] = false;
                    _next[value] = 0;
                }
            }
            catch (ArgumentException e)
            {
                //무시
            }
        }

        return returnValue;
    }
}