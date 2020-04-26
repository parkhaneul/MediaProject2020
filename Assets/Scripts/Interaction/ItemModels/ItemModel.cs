using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemKind
{
    Stone,
    Rock,
    Branch,
    Tree,
    Mountain
}
public struct ItemFormula
{
    public Dictionary<ItemKind, int> itemLists;
}
public abstract class ItemModel
{
    public abstract ItemKind GetKind();
    public abstract ItemFormula GetFormula();
}
