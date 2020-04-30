using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Carry,
    Running,
    UnCarry,
    Smash,
    Tool,
}

public class PlayerState : MonoBehaviour
{
    private CharacterAction _action;
    public Inventory Inventory;
    private PlayerStateEnum _state;
    public Tool equipment { get; private set; }

    public void Start()
    {
        if (Inventory == null)
            Inventory = gameObject.GetComponent<Inventory>();

        if (_action == null)
            _action = gameObject.GetComponent<CharacterAction>();
    }

    public void setState(PlayerStateEnum state)
    {
        _state = state;
    }

    public void setTool(Tool tool)
    {
        equipment = tool;
        
        _action.SetEquipment(tool);
    }

    public HashSet<Interactable> getInteractables()
    {
        return _action.interactables;
    }
}
