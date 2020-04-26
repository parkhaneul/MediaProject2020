using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : Interactable
{
    private MissionLogic ml;

    public void Start()
    {
        if (ml == null)
            ml = MissionLogic.Instance;
    }
    
    public override void OnInteract(CharacterAction actor)
    {
        var inven = actor.Inventory;

        bool checkItem = false;
        List<string> temp = new List<string>();
        
        foreach (var itemName in inven.getItemString())
        {
            var checking = ml.checkItemRequired(itemName);

            if (checking)
            {
                temp.Add(itemName);
            }

            checkItem |= checking;
        }

        if (checkItem)
        {
            foreach (var itemName in temp)
            {
                inven.deleteItem(itemName);
            }
            
            ml.addPercent(10);
        }
    }
}
