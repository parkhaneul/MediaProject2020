using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : MonoBehaviour, Placable
{
    public float speedValue = 5.0f;
    public float buffTime = 5.0f;
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
                buff();
            }
        }
    }

    private void buff()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;

        // GameObject go_particle = Instantiate(particle, transform.position + Vector3.up * 1.0f, Quaternion.identity);

        BuffLogic.Instance.addBuff(BuffKind.SpeedUp,(_) =>
        {
            foreach (var state in _)
            {
                state.speedMul *= speedValue;
                Logger.Log(state.name + " start buff speed " + state.speedMul);
            }   
        }, (_) =>
        {
            foreach (var state in _)
            {
                state.speedMul /= speedValue;
                Logger.Log(state.name + " end buff speed " + state.speedMul);
            }
        },buffTime,PlayerControlLogic.Instance.getAllPlayerState());
    }
    
    public void AdjustTransform(Grid grid)
    {
        transform.position = grid.gridCenter + new Vector3(0.0f, 0.00001f, 0.0f);
    }
}
