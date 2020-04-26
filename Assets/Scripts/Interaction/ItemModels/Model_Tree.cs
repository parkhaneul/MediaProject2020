using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Tree : ItemModel
{
    public override ItemKind GetKind()
    {
        return ItemKind.Branch;
    }
    public override ItemFormula GetFormula()
    {
        return new ItemFormula()
        {
            itemLists = new Dictionary<ItemKind, int>
            { 
                {ItemKind.Branch, 3}
            }
        };
    }
}