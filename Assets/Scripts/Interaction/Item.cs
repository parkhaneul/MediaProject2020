using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Placable
{
    public ItemKind kind;
    public Vector3 positionOffset = new Vector3(0.0f, 0.4f, 0.0f);
    
    public virtual void OnItemGet()
    {
        ObjectRecyclingLogic.Instance.chunk(name,gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                Inventory inven = other.transform.parent.GetComponent<Inventory>();
                if (inven != null && !inven.isFull())
                {
                    inven.addItem(this);
                    OnItemGet();
                }
<<<<<<< HEAD
=======
                //OnItemGet();
>>>>>>> 33209ef601cf18c652665688c08d7fffe52f4cfd
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
