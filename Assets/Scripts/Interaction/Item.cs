using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Placable
{
    public Vector3 positionOffset = new Vector3(0.0f, 0.4f, 0.0f);
    private BoxCollider collider;

    public void Start()
    {
        if (collider == null)
            collider = this.gameObject.GetComponent<BoxCollider>();
    }

    public virtual void OnItemGet()
    {
        ObjectRecyclingLogic.Instance.chunk(name,gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            PlayerState player = other.transform.parent.GetComponent<PlayerState>();
            if (player != null)
            {
                if (player.hasTool() == false)
                {
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

    public void AdjustPosition(Grid grid)
    {
        transform.position = grid.gridCenter + positionOffset;
    }
}
