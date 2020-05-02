using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private CharacterAction _action;
    public Inventory Inventory;
    public Tool equipment { get; private set; }

    public void Start()
    {
        if (Inventory == null)
            Inventory = gameObject.GetComponent<Inventory>();

        if (_action == null)
            _action = gameObject.GetComponent<CharacterAction>();
    }

    public void addItem(Item item)
    {
        Inventory.addItem(item);

        _action.getItem(item);
    }

    public void setTool(Tool tool)
    {
        equipment = tool;
        
        _action.SetEquipment(tool);
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
        return !(equipment == null);
    }

    public HashSet<Interactable> getInteractables()
    {
        return _action.interactables;
    }
}
