using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    public GameObject particleSocket;
    public GameObject particle;
    public int durability;
    public List<GameObject> dropItems;
    public ToolKind kind;
    private GameObject mParticle;
    
    public override bool OnInteract(PlayerState state)
    {
        ToolKind? kind = state.getToolKind();
        if(kind.HasValue)
        {
            if(kind.Value == this.kind)
            {
                OnDamaged();
                return true;
            }
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
        Grid grid = gridManager.GetRandomItemSpawnPosition(this);
        GameObject g = Instantiate(GetRandomItemToSpawn());
        Item item = g.GetComponent<Item>();
        if(item == null)
        {
            Debug.LogError("In dropItems All Gameobject should have Item Component");
        }
        item.AdjustTransform(grid);
        gridManager.OccupyPlacable(item, grid);
    }

    private void OnDamaged()
    {
        PlaySound();
        PlayParticle();
        if(--durability <= 0)
        {
            OnSpawnItem();
            OnDestroy();
            return;
        }
        effectManager.BlinkEffect(gameObject);
        effectManager.ShakeEffect(gameObject);
    }

    private void PlaySound()
    {
        if(kind == ToolKind.Pickax)
            SoundManager.Instance.PlayPickAxeSound(this.gameObject);
    }

    private void PlayParticle()
    {
        if(particle == null)
        {
            return;
        }

        if(mParticle != null)
        {
            ParticleSystem particleSystem = mParticle.GetComponent<ParticleSystem>();
            particleSystem.time = 0.0f;
            particleSystem.Play();
        }
        else
        {
            mParticle = Instantiate(particle, particleSocket.transform.position, Quaternion.identity);
            mParticle.transform.SetParent(gameObject.transform);
            ParticleSystem particleSystem = mParticle.GetComponent<ParticleSystem>();
            particleSystem.Play();
        }
    }
}
