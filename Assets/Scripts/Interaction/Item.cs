using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Placable
{
    public Vector3 positionOffset = new Vector3(0.0f, 0.4f, 0.0f);
    
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
                var inven = player.Inventory;
                if (inven != null)
                {
                    inven.addItem(this);
                }
                //OnItemGet();
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
