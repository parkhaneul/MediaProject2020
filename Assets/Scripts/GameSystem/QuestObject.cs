using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : Interactable
{
    private MissionLogic ml;

    public void Start()
    {
        base.Start();
        if (ml == null)
            ml = MissionLogic.Instance;
    }
    
    public override void OnInteract(PlayerState state)
    {
        Inventory inven = state.Inventory;
        List<int> removeItemIndices = new List<int>();
        for(int i = 0 ; i < inven.getItemsCount(); i ++)
        {
            Item item = inven.getItem(i);
            if(ml.isItemRequired(item.kind))
            {
                removeItemIndices.Add(i);
            }
        }
        removeItemIndices.Sort();
        removeItemIndices.Reverse();

        foreach(int i in removeItemIndices)
        {
            Item item = inven.popItem(i);
            ml.putItem(item);
        }
    }
}
