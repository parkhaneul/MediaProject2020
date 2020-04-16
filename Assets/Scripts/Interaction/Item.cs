using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public override void OnItemGet()
    {
        Debug.Log("Get Item : " + gameObject.name);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == CharacterAction.CONST_CharacterBound)
        {
            CharacterAction characterAction = other.transform.parent.GetComponent<CharacterAction>();
            if (characterAction != null)
            {
                OnItemGet();
            }
        }
    }
}
