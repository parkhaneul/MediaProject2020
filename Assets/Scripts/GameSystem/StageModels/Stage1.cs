using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : StageModel
{
    public override RequiredItems GetRequiredItems()
    {
        return new RequiredItems()
        {
            requiredItems = new Dictionary<ItemKind, int>
            { 
                {ItemKind.Tree, 1},
                {ItemKind.Rock, 1},
                {ItemKind.Mountain, 5}
            }
        };
    }
}
