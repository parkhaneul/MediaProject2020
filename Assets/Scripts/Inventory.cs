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

        //Inventory Test
        Observable.EveryUpdate()
            .Where(_ => _inventory.Count != 0)
            .Subscribe(_ =>
            {
                for (int i = 0; i < _inventory.Count; i++)
                {
                    var item = _inventory[i];
                    if(item.gameObject.active == false)
                        item.gameObject.SetActive(true);
                    item.transform.position = this.gameObject.transform.position + Vector3.up * 2;
                    item.transform.position += Vector3.right * (i - _inventory.Count/2);
                }
            })
            .AddTo(this);
    }

    public bool isFull()
    {
        return !(_inventory.Count < inventoryMaximumSize);
    }

    public void addItem(Item item)
    {
        if (_inventory.Count < inventoryMaximumSize)
        {
            _inventory.Add(item);
            Logger.Log(ToString());
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
            item.gameObject.SetActive(false);
        }
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
}
