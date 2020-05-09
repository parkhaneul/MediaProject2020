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
    protected override void Start()
    {
        base.Start();
        collider = GetComponent<BoxCollider>();
    }
    
    public void EquipmentMode()
    {
        collider.enabled = false;
    }

    public void GroundMode()
    {
        collider.enabled = true;
    }
    
    public override void OnInteract(PlayerState state)
    {
        state.setTool(this);
        owner = state;
        EquipmentMode();  

        owner.getInteractables().SetDirty(this);
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
            //조건 추가가 필요, 두 hit box의 union에서 벗어났을때 Dirty로 체크해야함.
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.SetDirty(this);
            }
        }
    }
}
