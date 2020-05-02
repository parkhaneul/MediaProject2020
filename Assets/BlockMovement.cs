using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    private MonoBehaviour[] scripts;
    private Vector3 basicScale;
    
    void Start()
    {
        scripts = GetComponents<MonoBehaviour>();
        basicScale = transform.localScale;
    }

    public void itemDrop(float time)
    {
        turn(false);
        StartCoroutine(reset(time));
    }
    
    public void turn(bool value)
    {
        foreach (var script in scripts)
        {
            if (script.GetType() != typeof(BlockMovement))
                script.enabled = value;
        }
    }

    IEnumerator reset(float time)
    {
        yield return new WaitForSeconds(time);
        turn(true);
        this.transform.localScale = basicScale;
    }
}
