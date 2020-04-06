using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public float speed;
    public void move(Point point)
    {
        turn(point);
        this.gameObject.transform.position += this.gameObject.transform.forward * speed;
    }

    public void turn(Point point)
    {
        var lookAtVector = this.transform.position + new Vector3(point.x, point.y, point.z) * speed;
        this.gameObject.transform.LookAt(lookAtVector);
    }

    public void action(bool action)
    {
        Debug.Log("action");
    }
}
