using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    public int durability;
    public List<GameObject> dropItems;
    public ToolKind kind;
    public override void OnInteract(CharacterAction actor)
    {
        if(actor.equipment.kind == kind)
        {
            OnDamaged();
        }
        else
        {

        }
    }

    private GameObject GetRandomItemToSpawn()
    {
        if(dropItems != null && dropItems.Count > 0)
            return dropItems[Random.Range(0, dropItems.Count)];
        return null;
    }
    private void OnSpawnItem()
    {
        Grid grid = gridManager.GetRandomItemSpawnPosition(this);
        GameObject g = Instantiate(GetRandomItemToSpawn());
        Item item = g.GetComponent<Item>();
        if(item == null)
        {
            Debug.LogError("In dropItems All Gameobject should have Item Component");
        }
        item.AdjustPosition(grid);
        grid.Occupy(item);
    }
    private void OnDamaged()
    {
        OnSpawnItem();
        if(--durability <= 0)
        {
            OnDestroy();
            return;
        }
        effectManager.BlinkEffect(gameObject);
        effectManager.ShakeEffect(gameObject);
    }
}
