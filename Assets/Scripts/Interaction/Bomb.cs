using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, Placable
{
    public float radius = 5.0f;
    public float knockOutTime = 5.0f;
    public GameObject particle;
    public GameObject stunnedParticle;
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
        
        //TODO : Don't call 'Find' methods during run time. 
        CharacterAction[] characters = FindObjectsOfType<CharacterAction>();
        foreach(var ch in characters)
        {
            if(Vector3.SqrMagnitude(ch.transform.position - transform.position) < radius * radius)
            {
                KnockOut(ch);
            }
        }
    }

    private void KnockOut(CharacterAction character)
    {
        character.isKnockOuted = true;
        GameObject stunFx = Instantiate(stunnedParticle);
        stunFx.transform.parent = character.gameObject.transform;
        stunFx.transform.localPosition = Vector3.zero + Vector3.up;
        
        StartCoroutine("KnockOutTimer",character);
    }

    public void AdjustTransform(Grid grid)
    {
        transform.position = grid.gridCenter + new Vector3(0.0f, 0.00001f, 0.0f);
    }

    IEnumerator KnockOutTimer(CharacterAction character)
    {
        yield return new WaitForSeconds(knockOutTime);
        character.isKnockOuted = false;
    }
}
