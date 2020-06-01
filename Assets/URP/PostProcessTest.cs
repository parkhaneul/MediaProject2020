using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.PostProcessing;

using UnityEngine;

[ExecuteInEditMode]
[Serializable]
[PostProcess(typeof(PostProcessTestRenderer), PostProcessEvent.AfterStack, "Custom/SpidermanNewUniverse")]
public class PostProcessTest : PostProcessEffectSettings
{
    
}

sealed public class PostProcessTestRenderer : PostProcessEffectRenderer<PostProcessTest>
{
    public override void Render(PostProcessRenderContext context)
    {
        
    }
}