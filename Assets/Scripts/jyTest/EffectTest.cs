using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    public GameObject RockObject;
    public GameObject TreeObject;
    public GameObject RandomBoxObject;
    
    public ParticleSystem RockEffectModel;
    public ParticleSystem TreeEffectModel;
    public ParticleSystem RandomBoxEffectModel;

    public void PlayRockEffect()
    {
        if (RockObject.activeSelf)
        {
            RockObject.gameObject.SetActive(false);
            ParticleSystem ef = GameObject.Instantiate(RockEffectModel);
            ef.transform.position = RockObject.transform.position;
            ef.gameObject.SetActive(true);
            ef.Play();
            StartCoroutine(Reset(RockObject));
            GameObject.Destroy(ef, 4);
        }
    }
    public void PlayTreeEffect()
    {
        if (TreeObject.activeSelf)
        {
            TreeObject.gameObject.SetActive(false);
            ParticleSystem ef = GameObject.Instantiate(TreeEffectModel);
            ef.transform.position = TreeObject.transform.position;
            ef.gameObject.SetActive(true);
            ef.Play();
            StartCoroutine(Reset(TreeObject));
            GameObject.Destroy(ef, 4);
        }
    }
    public void PlayRandomBoxEffect()
    {
        if (RandomBoxObject.activeSelf)
        {
            RandomBoxObject.gameObject.SetActive(false);
            ParticleSystem ef = GameObject.Instantiate(RandomBoxEffectModel);
            ef.transform.position = RandomBoxObject.transform.position;
            ef.gameObject.SetActive(true);
            ef.Play();
            StartCoroutine(Reset(RandomBoxObject));
            GameObject.Destroy(ef, 4);
        }
    }

    IEnumerator Reset(GameObject go)
    {
        yield return new WaitForSeconds(4);

        go.SetActive(true);
    }
}