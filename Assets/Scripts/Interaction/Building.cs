using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    public override void OnInteract()
    {
        OnDamaged();
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

        if(--durability <= 0)
        {
            OnDestroy();
        }
    }
}
