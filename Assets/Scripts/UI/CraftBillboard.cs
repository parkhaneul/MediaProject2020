using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CraftBillboard : MonoBehaviour
{
    public Texture defaultTexture;
    public List<Texture> itemTextures;
    public List<UnityEngine.Grid> grids;
    public CraftBox craftBox;
    public RawImage resultImage;
    Canvas canvas;
    GridLayout gridLayout;
    private Dictionary<ItemKind, Texture> itemKindToTexture;
    
    public void Synchronize()
    {
        Synchronize(craftBox.items, craftBox.upcomingResult);
    }

    private void Synchronize(List<Item> items, ItemKind? result)
    {

        for(int i = 0 ; i < grids.Count; i++)
        {
            RawImage img = grids[i].GetComponent<RawImage>();
            if(items == null || items.Count <= i)
            {
                img.texture = defaultTexture;
            }
            else
            {
                img.texture = itemKindToTexture[items[i].kind];
            }
        }
        resultImage.texture = result == null ? null : itemKindToTexture[result.Value];
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        gridLayout = GetComponentInChildren<GridLayout>();
        if(canvas == null || gridLayout == null || craftBox == null)
        {
            Debug.LogError("Initializing Craft Billboard has failed");
        }

        itemKindToTexture = new Dictionary<ItemKind, Texture>(){
            {ItemKind.Stone, itemTextures[0]},
            {ItemKind.Branch, itemTextures[1]},
            {ItemKind.Rock, itemTextures[2]},
            {ItemKind.Tree, itemTextures[3]},
            {ItemKind.Mountain, itemTextures[4]}
        };

        Synchronize(null, null);
    }

    void Update()
    {
        //gameObject.transform.LookAt(gameObject.transform.position + canvas.worldCamera.transform.forward, Vector3.up);
    }
}
