using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementSystem : MonoBehaviour
{
    private static ObjectMovementSystem _instance;
    public static ObjectMovementSystem Instance
    {
        get
        {;
            return _instance;
        }
    }

    public const float g = 9.8f;
    
    private float tx;
    private float ty;
    private float tz;
    private float v;
    private float elapsed_time;
    private float max_height;
    private float t;
    private Vector3 start_pos;
    private Vector3 end_pos;

    private float dat;

    public ObjectMovementSystem()
    {
        if (_instance == null)
            _instance = this;
    }
    
    public void shoot(GameObject target, Vector3 direction,float power, float max_height, System.Action onComplete)
    {
        turn(target,false);
        
        start_pos = target.transform.position;
        end_pos = start_pos + direction * power;

        this.max_height = max_height;

        var dh = end_pos.y - start_pos.y;
        var mh = this.max_height - start_pos.y;
        ty = Mathf.Sqrt(2 * g * mh);

        float a = g;
        float b = -2 * ty;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        
        tx = -(start_pos.x - end_pos.x) / dat;
        tz = -(start_pos.z - end_pos.z) / dat;

        this.elapsed_time = 0;

        StartCoroutine(this.ShootImpl(target, onComplete));
    }

    public void itemDrop(GameObject target, float time)
    {
        turn(target, false);
        StartCoroutine(reset(target,time));
    }
    
    public void turn(GameObject target, bool value)
    {
        var scripts = target.GetComponents<MonoBehaviour>();

        foreach (var script in scripts)
        {
            script.enabled = value;
        }
    }

    IEnumerator ShootImpl(GameObject target, System.Action onComplete)
    {
        while (true)
        {
            var tx = start_pos.x + this.tx * elapsed_time;
            var ty = start_pos.y + this.ty * elapsed_time - 0.5f * g * elapsed_time * elapsed_time;
            var tz = start_pos.z + this.tz * elapsed_time;

            var tpos = new Vector3(tx, ty, tz);
            
            target.transform.LookAt(tpos);
            target.transform.position = tpos;

            this.elapsed_time += Time.deltaTime;

            if (this.elapsed_time >= this.dat)
                break;

            yield return null;
        }

        onComplete();
    }

    IEnumerator reset(GameObject target,float time)
    {
        yield return new WaitForSeconds(time);
        turn(target, true);
        this.transform.localScale = Vector3.one;
    }
}
