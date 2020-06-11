using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

/// <summary>
/// change code if Item Data Class Update
/// </summary>
public class Inventory : MonoBehaviour
{
    private List<Item> _inventory;
    public int inventoryMaximumSize;
    
    public void Start()
    {
        if (_inventory == null)
            _inventory = new List<Item>();
    }

    public bool isFull()
    {
        return !(_inventory.Count < inventoryMaximumSize);
    }

    ///<summary>
    ///Don't call this function outside of CharacterAction.cs 
    ///</summary>
    public void addItem(Item item)
    {
        if (_inventory.Count < inventoryMaximumSize)
        {
            SoundManager.Instance.PlayPickItemSound(this.gameObject);
            _inventory.Add(item);
        }
        else
        {
            Logger.LogError("Inventory is Full.");
        }
    }

    public void deleteItem(string itemName)
    {
        List<Item> items = new List<Item>();

        foreach (var item in _inventory)
        {
            if (item.name == itemName)
            {
                items.Add(item);
            }
        }

        foreach (var item in items)
        {
            _inventory.Remove(item);
            ObjectRecyclingLogic.Instance.chunk(item.name,item.gameObject);
        }
    }

    [CanBeNull]
    public Item putItem(int index)
    {
        if (_inventory == null)
            return null;

        if (_inventory.Count < index)
            return null;

        var item = _inventory[index];
        _inventory.RemoveAt(index);

        return item;
    }

    [CanBeNull]
    public Item getItem(int index)
    {
        if (_inventory == null)
            return null;
        
        if (_inventory.Count < index)
            return null;

        return _inventory[index];
    }

    public int getItemsCount()
    {
        if (_inventory == null)
            return 0;
            
        return _inventory.Count;
    }
    
    [CanBeNull]
    public Item popItem(int index)
    {
        if (_inventory == null)
            return null;
        
        if (_inventory.Count < index)
            return null;

        Item item = _inventory[index];
        Destroy(item.gameObject);
        _inventory.RemoveAt(index);
        return item;
    }

    [CanBeNull]
    public Item[] popAllItems()
    {
        if (_inventory == null)
            return null;
        
        if (_inventory.Count < 0)
            return null;

        Item[] items = _inventory.ToArray();
        foreach(var item in items)
            Destroy(item.gameObject);
        _inventory.Clear();
        return items;
    }

    public List<string> getItemString()
    {
        List<string> returnValue = new List<string>();

        foreach (var item in _inventory)
        {
            returnValue.Add(item.name);
        }

        return returnValue;
    }
    
    public override string ToString()
    {
        return "Inventory item count is " + _inventory.Count;
    }

    public int getItemCount()
    {
        return _inventory.Count;
    }
}
