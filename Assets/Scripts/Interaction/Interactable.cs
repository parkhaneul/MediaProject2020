using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour, Placable
{
    static protected EffectManager effectManager;
    static protected GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        effectManager = EffectManager.Instance;   
        gridManager = GridManager.Instance;
    }

    public abstract void OnInteract(CharacterAction actor);
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

    public void AdjustPosition(Grid grid)
    {
        throw new System.NotImplementedException();
    }
}
