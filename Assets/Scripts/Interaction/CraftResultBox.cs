using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This class is highly dependent on CraftBox class.
///</summary>
public class CraftResultBox : Interactable
{
    public CraftBox craftBox;

    public override void OnInteract(PlayerState state)
    {
        if(craftBox == null)
        {
            Debug.LogError("Craft Result Box should have a reference of CraftBox");
            return;
        }

        if(state.Inventory.isFull())
        {
            return;
        }

        Item result = craftBox.Craft();
        state.Inventory.addItem(result);
        result.OnItemGet();
    }

    
}
