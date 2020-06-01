using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float speedMul = 1;

    public bool isJoined = false;
    
    private CharacterAction _action;
    public Inventory Inventory;

    public void Start()
    {
        if (Inventory == null)
            Inventory = gameObject.GetComponent<Inventory>();

        if (_action == null)
            _action = gameObject.GetComponent<CharacterAction>();
    }

    public void connect(bool value)
    {
        var rb = this.GetComponent<Rigidbody>();
        
        if (!value)
        {
            this.transform.position += Vector3.up * 10;
        }

        isJoined = value;
        this.gameObject.SetActive(value);
    }

    public void addItem(Item item)
    {
        Inventory.addItem(item);

        _action.getItem(item);
    }

    public void setTool(Tool tool)
    {
        _action.SetEquipment(tool);
    }
    public bool isInventoryFull()
    {
        return Inventory.isFull();
    }
    public Item putItem(int index)
    {
        return Inventory.putItem(index);
    }

    public bool hasItem()
    {
        return getItemCount() > 0;
    }

    public int getItemCount()
    {
        return Inventory.getItemCount();
    }

    public bool hasTool()
    {
        return !(_action.equipment == null);
    }

    public ToolKind? getToolKind()
    {
        if(!hasTool())
            return null;
        return _action.equipment.kind;
    }

    public InteractableSet getInteractables()
    {
        return _action.interactables;
    }

    public override bool Equals(object other)
    {
        if (other == null)
            return false;

        if (other.GetType() != typeof(PlayerState))
            return false;

        if (((PlayerState) other).gameObject == this.gameObject)
            return true;

        return false;
    }
}
