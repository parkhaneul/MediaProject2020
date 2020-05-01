using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactable : MonoBehaviour, Placable
{
    static protected EffectManager effectManager;
    static protected GridManager gridManager;
    protected HashSet<CharacterAction> characters;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        effectManager = EffectManager.Instance;   
        gridManager = GridManager.Instance;
        characters = new HashSet<CharacterAction>();
    }

    public abstract void OnInteract(PlayerState state);
    protected virtual void OnDestroy() 
    {
        foreach(var character in characters)
        {
            character.interactables.Remove(this);
        }
        // Debug.Log(gameObject.name + " : Destroyed");
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Add(this);
                characters.Add(characterAction);
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
                characters.Remove(characterAction);
            }
        }
    }

    public void AdjustPosition(Grid grid)
    {
        throw new System.NotImplementedException();
    }
}
