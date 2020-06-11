using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : Interactable
{
    public List<GameObject> dropItems;

    public override bool OnInteract(PlayerState state)
    {
        ToolKind? kind = state.getToolKind();
        if(kind.HasValue)
        {
            OnSpawnItem();
            OnDestroy();
            return true;
        }
        return false;
    }

    private GameObject GetRandomItemToSpawn()
    {
        if(dropItems != null && dropItems.Count > 0)
            return dropItems[Random.Range(0, dropItems.Count)];
        return null;
    }

    private void OnSpawnItem()
    {
        GridBundle bundle = gridManager.GetGridBundles(this);
        if(bundle.grids.Count != 1)
        {
            Debug.LogError("Random Box Grid Error");
            return;
        }

        Grid grid = bundle.grids[0];
        GameObject g = Instantiate(GetRandomItemToSpawn());

        Placable item = g.GetComponent<Placable>();
        if(item == null)
        {
            Debug.LogError("Error : Random Box");
        }

        item.AdjustPosition(grid);
        gridManager.UnoccupyPlacable(this);
        gridManager.OccupyPlacable(item, grid);
    }
}
