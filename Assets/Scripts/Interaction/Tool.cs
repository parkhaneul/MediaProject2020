using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolKind
{
    Axe,
    Pickax,
    Sword
}

public class Tool : Interactable
{
    public ToolKind kind;
    private PlayerState owner;
    private BoxCollider collider;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }
    
    public override void OnInteract(PlayerState state)
    {
        state.setTool(this);
        owner = state;
        collider.enabled = false;

        owner.getInteractables().Remove(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox || other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Add(this);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox || other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Remove(this);
            }
        }
    }
}
