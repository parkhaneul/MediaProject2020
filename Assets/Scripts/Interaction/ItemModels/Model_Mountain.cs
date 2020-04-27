using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Mountain : ItemModel
{
    public override ItemKind GetKind()
    {
        return ItemKind.Mountain;
    }
    public override ItemFormula GetFormula()
    {
        return new ItemFormula()
        {
            itemLists = new Dictionary<ItemKind, int>
            { 
                {ItemKind.Rock, 2},
                {ItemKind.Tree, 2},
                {ItemKind.Stone, 1},
                {ItemKind.Branch, 1}
            }
        };
    }
}

