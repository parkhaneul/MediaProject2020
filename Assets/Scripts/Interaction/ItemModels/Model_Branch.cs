using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Branch : ItemModel
{
    public override ItemKind GetKind()
    {
        return ItemKind.Branch;
    }
    public override ItemFormula GetFormula()
    {
        return new ItemFormula();
    }
}
