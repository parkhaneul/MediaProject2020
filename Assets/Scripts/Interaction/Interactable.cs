using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
    static protected EffectManager effectManager;
    public int durability;
    public List<Item> dropItems;

    // Start is called before the first frame update
    void Start()
    {
        effectManager = EffectManager.Instance;   
    }

    public abstract void OnInteract(CharacterAction actor); //make this abstract
    protected virtual void OnDestroy() 
    {
        //Debug.Log(gameObject.name + " : Destroyed");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Add(this);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Remove(this);
            }
        }
    }
}
