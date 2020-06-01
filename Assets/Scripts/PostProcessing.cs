using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    public Material postProcessingMat;
    [Range(0, 0.5f)]
    public float chromatic = 0.2f;

    /// <summary>
    /// OnRenderImage is called after all rendering is complete to render image.
    /// </summary>
    /// <param name="src">The source RenderTexture.</param>
    /// <param name="dest">The destination RenderTexture.</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(postProcessingMat != null)
        {
            postProcessingMat.SetFloat("_RGBSplit", chromatic);
            Graphics.Blit(src, dest, postProcessingMat);
        }
    }

}
