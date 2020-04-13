using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    static private EffectManager effectManager;
    public int durability;
    public List<Item> dropItems;

    // Start is called before the first frame update
    void Start()
    {
        effectManager = EffectManager.Instance;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDamage()
    {
        effectManager.BlinkEffect(gameObject);
        effectManager.ShakeEffect(gameObject);

        if(--durability <= 0)
        {
            OnDestroy();
        }
    }

    public virtual void OnDestroy() 
    {
        //Debug.Log(gameObject.name + " : Destroyed");
    }

    protected virtual void OnSpawnItem()
    {
        //Debug.Log(gameObject.name + " : SpawnItem");
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterAction characterAction = other.GetComponent<CharacterAction>();
        if (characterAction != null)
        {
            characterAction.interactables.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterAction characterAction = other.GetComponent<CharacterAction>();
        if (characterAction != null)
        {
            characterAction.interactables.Remove(this);
        }
    }

    private Item GetRandomItemToSpawn()
    {
        if(dropItems != null && dropItems.Count > 0)
            return dropItems[Random.Range(0, dropItems.Count)];
        return null;
    }

}
