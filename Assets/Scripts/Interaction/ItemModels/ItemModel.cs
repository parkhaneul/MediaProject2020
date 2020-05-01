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

    static public ItemFormula? GetFormula(ItemKind kind)
    {
        switch(kind)
        {
            case ItemKind.Stone : 
                return new Model_Stone().GetFormula();
            case ItemKind.Rock : 
                return new Model_Rock().GetFormula();
            case ItemKind.Branch : 
                return new Model_Branch().GetFormula();
            case ItemKind.Tree : 
                return new Model_Tree().GetFormula();
            case ItemKind.Mountain : 
                return new Model_Mountain().GetFormula();
        }
        return null;
    }
}
