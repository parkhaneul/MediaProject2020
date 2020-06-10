using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, Placable
{
    public GameObject particle;
    private float particleDuration = 4.0f;
    private MeshRenderer meshRenderer;
    private BoxCollider collider;
    private static GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.Instance;
        collider = gameObject.GetComponent<BoxCollider>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
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
                gridManager.UnoccupyPlacable(this);
                Explode();
            }
        }
    }

    private void Explode()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;

        GameObject go_particle = Instantiate(particle, transform.position + Vector3.up * 1.0f, Quaternion.identity);
        // 1. 반지름 5 안의 캐릭터들을 가져온다.
        // 2. 캐릭터들을 n초간 기절시킨다.
        // 3. 
        
    }

    public void AdjustPosition(Grid grid)
    {
        throw new System.NotImplementedException();
    }
}
