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
        Logger.Log("ResultBox 1");
        
        if(craftTable == null)
        {
            Debug.LogError("Craft Result Box should have a reference of CraftBox");
            return false;
        }
        
        Logger.Log("ResultBox 2");

        if(state.Inventory.isFull() || state.hasTool())
        {
            return false;
        }
        
        Logger.Log("ResultBox 3");
        
        GameObject craftedItem = craftTable.Craft();
        if(craftedItem == null)
            return false;
        
        Logger.Log("ResultBox 4");
        
        Item item = craftedItem.GetComponent<Item>();
        
        state.addItem(item);
        
        return true;
    }
}
