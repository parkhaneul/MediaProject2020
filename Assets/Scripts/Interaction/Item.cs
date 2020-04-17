using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void OnItemGet()
    {
        Debug.Log("Get Item : " + gameObject.name);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                Inventory inven = other.transform.parent.GetComponent<Inventory>();
                if (inven != null)
                {
                    inven.addItem(this);
                }
                OnItemGet();
            }
        }
    }
}
