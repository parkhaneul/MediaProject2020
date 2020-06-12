using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CraftTable : Interactable
{
    public const int craftBoxMaximumSize = 6;
    private static CraftSystem craftSystem;

    protected override void Start()
    {
        base.Start();
        craftSystem = CraftSystem.Instance;
    }

    public GameObject Craft()
    {
        return CraftSystem.Instance.craft();
    }

    private Item[] GetItemsFromInventory(Inventory inventory)
    {
        if(craftBoxMaximumSize - CraftSystem.Instance.materials.Count >= inventory.getItemsCount())
        {
            return inventory.popAllItems();
        }
        
        Item[] itemsFromInventory = new Item[craftBoxMaximumSize - CraftSystem.Instance.materials.Count];
        for(int i = 0 ; i < itemsFromInventory.Length; i++)
        {
            itemsFromInventory[i] = inventory.popItem(i);
        }
        
        return itemsFromInventory;
    }
    private void insertItem(params Item[] items)
    {
        CraftSystem.Instance.addItems(items.Select(_ => _.getData()).ToArray());
    }

    public override bool OnInteract(PlayerState state)
    {
        Item[] item = GetItemsFromInventory(state.Inventory);
        
        if(item.Length > 0)
            insertItem(item);
        else
        {
            var pickUpItem = CraftSystem.Instance.pickUp();
            Item pitem = pickUpItem.GetComponent<Item>();
        
            state.addItem(pitem);
            Logger.Log("pickup Item");
        }
        return true;
    }
}
