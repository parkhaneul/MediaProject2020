using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This class is highly dependent on CraftBox class.
///</summary>
public class CraftResultBox : Interactable
{
    public CraftTable craftTable;

    public override bool OnInteract(PlayerState state)
    {
        if(craftTable == null)
        {
            Debug.LogError("Craft Result Box should have a reference of CraftBox");
            return false;
        }

        if(state.Inventory.isFull())
        {
            return false;
        }

        GameObject craftedItem = craftTable.Craft();
        if(craftedItem == null)
            return false;
        
        Item item = craftedItem.GetComponent<Item>();
        
        state.addItem(item);
        item.OnItemGet();

        return true;
    }

    
}
