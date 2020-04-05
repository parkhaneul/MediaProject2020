using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour //ToDo : If it possible, make this static class
{
    private static EffectManager instance;
    public static EffectManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
        else 
        {
            DestroyImmediate(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlinkEffect(GameObject target, Color? tintColor = null, float time = 0.2f, int count = 2)
    {
        tintColor = tintColor ?? Color.red;
        IEnumerator coroutine = BlinkGameObject(target, tintColor.Value, time, count);
        StartCoroutine(coroutine);
    }

    public void ShakeEffect(GameObject target, float time = .3f, float speed = 30.0f, float amount = 0.05f)
    {
        IEnumerator coroutine = ShakeGameObject(target, time, speed, amount);
        StartCoroutine(coroutine);
    }

    private IEnumerator BlinkGameObject(GameObject target, Color color, float time, int count)
    {
        int iteration = 0;
        float waitTime = time / (count * 2);
        MeshRenderer s_meshRenderer = target.GetComponent<MeshRenderer>();
        if(s_meshRenderer == null)
        {
            yield return null;
        }

        List<Color> originalColor = new List<Color>();
        Material[] materials = s_meshRenderer.materials;
        foreach(Material material in materials)
        {
            originalColor.Add(material.color);
        }

        while(iteration++ < count)
        {
            for(int i = 0 ; i < materials.Length; i++)
            {
                materials[i].color = Color.Lerp(color, originalColor[i], 0.2f);
            }
            yield return new WaitForSeconds(waitTime);
            for(int i = 0 ; i < materials.Length; i++)
            {
                materials[i].color = originalColor[i];
            }
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    private IEnumerator ShakeGameObject(GameObject target, float time, float speed, float amount)
    {
        Debug.Log("Shake");
        Vector3 originalPos = target.transform.position;
        float startTime =Time.time;
        while(Time.time - startTime < time)
        {
            Vector3 vec = target.transform.position;
            vec.x += Mathf.Sin(Time.time * speed) * amount;
            target.transform.position = vec;
            yield return new WaitForEndOfFrame();
        }
        target.transform.position = originalPos;
        yield return null;
    }
}
