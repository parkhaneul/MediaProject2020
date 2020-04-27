using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Stone : ItemModel
{
    public override ItemKind GetKind()
    {
        return ItemKind.Stone;
    }
    public override ItemFormula GetFormula()
    {
        return new ItemFormula();
    }
}
