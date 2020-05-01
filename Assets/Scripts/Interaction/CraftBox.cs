using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBox : Interactable
{
    public CraftBillboard billboard;
    [HideInInspector]
    public List<Item> items;
    [HideInInspector]
    public ItemKind? upcomingResult;
    public int craftBoxMaximumSize;
    void InsertItem(params Item[] items)
    {   
        this.items.AddRange(items);
        upcomingResult = CalculateCloesetResult();
        billboard.Synchronize();
    }
    
    void DrawItem()
    {
        
        billboard.Synchronize();
    }

    void Craft()
    {
        // ItemFormula formula;
        billboard.Synchronize();
    }

    ItemKind? CalculateCloesetResult()
    {
        // 1. 현재 inventory status를 계산
        Dictionary<ItemKind, int> inventoryStatus = new Dictionary<ItemKind, int>();
        foreach(var item in items)
        {
            if(inventoryStatus.ContainsKey(item.kind))
            {
                inventoryStatus[item.kind] = inventoryStatus[item.kind] + 1;
            }
            else
            {
                inventoryStatus.Add(item.kind, 1);
            }
        }

        // 2. Top tier 부터 순차적으로 내려온다. 조합이 복잡하고, 다른 조합법을 부분집합으로 갖는 super set이 tier가 높아야한다. 
        if(isCraftable(new Model_Tree().GetFormula(), inventoryStatus))
        {
            return ItemKind.Tree;
        }
        else if(isCraftable(new Model_Rock().GetFormula(), inventoryStatus))
        {
            return ItemKind.Rock;
        }
        
        // 3. 끝까지 맞는 조합이 없으면 null
        return null;
    }

    bool isCraftable(ItemFormula formula, Dictionary<ItemKind, int> inventoryStatus)
    {
        if(formula.itemLists == null)
            return false;
        
        foreach(var ingredient in formula.itemLists)
        {
            int val;
            if(inventoryStatus.TryGetValue(ingredient.Key, out val))
            {
                if(val != ingredient.Value)
                    return false;
            }
            else 
            {
                return false;
            }
        }

        //
        // Let's assume we can make 'tree' with 3 'branches'. what if there are 4 'branches'?
        // For now, in this case we return false. 
        // 
        if(formula.itemLists.Count != inventoryStatus.Count) 
            return false;

        return true;
    }

    Item[] GetItemsFromInventory(Inventory inventory)
    {
        if(craftBoxMaximumSize - items.Count >= inventory.getItemsCount())
        {
            return inventory.popAllItems();
        }
        Item[] itemsFromInventory = new Item[craftBoxMaximumSize - items.Count];
        for(int i = 0 ; i < itemsFromInventory.Length; i++)
        {
            itemsFromInventory[i] = inventory.popItem(i);
        }
        return itemsFromInventory;
    }

    protected override void Start()
    {
        base.Start();
        items = new List<Item>();
    }

    public override void OnInteract(PlayerState state)
    {
        Item[] items = GetItemsFromInventory(state.Inventory);
        InsertItem(items);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox || other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Add(this);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_InteractionHitBox || other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                characterAction.interactables.Remove(this);
            }
        }
    }
}
