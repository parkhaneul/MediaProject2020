using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Rock : ItemModel
{
    public override ItemKind GetKind()
    {
        return ItemKind.Rock;
    }
    public override ItemFormula GetFormula()
    {
        return new ItemFormula()
        {
            itemLists = new Dictionary<ItemKind, int>
            { 
                {ItemKind.Stone, 3}
            }
        };
    }
}
