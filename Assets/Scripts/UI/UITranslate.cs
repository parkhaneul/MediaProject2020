using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITranslate : MonoBehaviour
{
    public bool isSpin;
    public bool isCCW;
    public int spinSpeed;

    public bool isBlink;
    public float tikTime;
    
    private void Start()
    {
        if (isBlink)
            StartCoroutine("blink");

        if (isSpin)
            StartCoroutine("spin");
    }

    IEnumerator spin()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        var value = isCCW ? 1 : -1;
        while (true)
        {
            this.transform.eulerAngles += Vector3.forward * value * spinSpeed;
            yield return wait;
        }
    }

    IEnumerator blink()
    {
        WaitForSeconds wait = new WaitForSeconds(tikTime);
        var gList = this.GetComponents<UIBehaviour>();

        while (true)
        {
            foreach (var g in gList)
            {
                g.enabled = !g.enabled;
            }

            yield return wait;
        }
    }
}
