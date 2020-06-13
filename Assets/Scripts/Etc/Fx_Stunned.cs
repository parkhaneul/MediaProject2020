using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Stunned : MonoBehaviour
{
    public float timeToDie = 5.0f;
    private float startTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > startTime + timeToDie)  
            Destroy(gameObject); 
    }
}
