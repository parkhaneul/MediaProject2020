using System;
using System.Collections.Generic;
using System.Linq;
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
    
    private Repeater _key = new Repeater("Horizontal","","Vertical");
    private Repeater _action = new Repeater("Fire1");
    
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
    }
}

public class Repeater
{
    private const float threshold = 0.1f;
    private const float rate = 0.05f;
    private float _next;
    private bool _hold;
    private string[] _axisArray;
    
    public Repeater(params string[] axisNames)
    {
        _axisArray = axisNames;
    }

    public bool getBoolean()
    {
        bool returnValue = false;
        if (_axisArray.Length == 1)
        {
            float value = Input.GetAxisRaw(_axisArray.First());
            returnValue = value > 0;
        }
        return returnValue;
    }

    public Point getPoint()
    {
        float xValue = Input.GetAxisRaw(_axisArray[0]);
        float zValue = Input.GetAxisRaw(_axisArray[2]);

        if (xValue == 0 && zValue == 0)
            return null;
        
        return new Point(xValue,0,zValue);
    }

    public float getValue(float value)
    {
        float returnValue = 0;
        
        if (value != 0)
        {
            if (Time.time > _next)
            {
                returnValue = value;

                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return returnValue;
    }
}