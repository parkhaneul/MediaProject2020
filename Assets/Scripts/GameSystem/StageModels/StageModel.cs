using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RequiredItems
{
    public Dictionary<ItemKind, int> requiredItems;
}
public abstract class StageModel
{
    public abstract RequiredItems GetRequiredItems();
}
