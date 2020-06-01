using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This class is highly dependent on CraftBox class.
///</summary>
public class CraftResultBox : Interactable
{
    public CraftTable craftTable;

    public override void OnInteract(PlayerState state)
    {
        if(craftTable == null)
        {
            Debug.LogError("Craft Result Box should have a reference of CraftBox");
            return;
        }

        if(state.Inventory.isFull())
        {
            return;
        }

        GameObject craftedItem = craftTable.Craft();
        if(craftedItem == null)
            return;
        
        Item item = craftedItem.GetComponent<Item>();
        
        state.addItem(item);
        item.OnItemGet();
    }

    
}
