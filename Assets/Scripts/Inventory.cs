using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> _inventory;
    public int inventoryMaximumSize;
    
    public void Start()
    {
        if (_inventory == null)
            _inventory = new List<Item>();

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

    [CanBeNull]
    public Item getItem(int index)
    {
        if (_inventory == null)
            return null;
        
        if (_inventory.Count < index)
            return null;

        return _inventory[index];
    }
    public override string ToString()
    {
        return "Inventory item count is " + _inventory.Count;
    }
}
