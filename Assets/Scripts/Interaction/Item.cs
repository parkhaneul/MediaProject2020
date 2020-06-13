using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Placable
{
    public int ItemCode;
    public ItemKind kind;
    public Vector3 positionOffset = new Vector3(0.0f, 0.4f, 0.0f);
    public Vector3 scaleOffset = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 rotationOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private BoxCollider collider;

    private static GridManager gridManager;

    public void Start()
    {
        gridManager = GridManager.Instance;
        if (collider == null)
            collider = this.gameObject.GetComponent<BoxCollider>();
    }
    
    public virtual void OnItemGet()
    {
        // ObjectRecyclingLogic.Instance.chunk(name,gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;
        
        if(other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            PlayerState player = other.transform.parent.GetComponent<PlayerState>();
            if (player != null)
            {
                if (player.hasTool() == false && !player.isInventoryFull())
                {
                    gridManager.UnoccupyPlacable(this);
                    player.addItem(this);
                    //OnItemGet();
                }
                else
                    Logger.Log("Player has Tools");
            }
        }
    }

    public override string ToString()
    {
        return gameObject.name;
    }

    public void AdjustTransform(Grid grid)
    {
        //Adjust Transform when Item is assigned to grid.
        transform.position = grid.gridCenter + positionOffset;
        transform.localScale = scaleOffset;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    public ItemData getData()
    {
        return ItemPalette.Instance.searchItem(ItemCode);
    }
}
