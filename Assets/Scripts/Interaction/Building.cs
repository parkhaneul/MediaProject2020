using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    public int durability;
    public List<Item> dropItems;
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

    private Item GetRandomItemToSpawn()
    {
        if(dropItems != null && dropItems.Count > 0)
            return dropItems[Random.Range(0, dropItems.Count)];
        return null;
    }
    private void OnSpawnItem()
    {
        //Debug.Log(gameObject.name + " : SpawnItem");
    }
    private void OnDamaged()
    {
        effectManager.BlinkEffect(gameObject);
        effectManager.ShakeEffect(gameObject);
        OnSpawnItem();
        if(--durability <= 0)
        {
            OnDestroy();
        }
    }
}
