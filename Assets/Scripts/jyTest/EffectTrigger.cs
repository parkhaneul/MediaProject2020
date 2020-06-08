using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EffectTest))]
public class EffectTrigger : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EffectTest effectTest = (EffectTest)target;
        if (GUILayout.Button("PlayRockEffect"))
        {
            effectTest.PlayRockEffect();
        }
        if (GUILayout.Button("PlayTreeEffect"))
        {
            effectTest.PlayTreeEffect();
        }
        if (GUILayout.Button("PlayRandomBoxEffect"))
        {
            effectTest.PlayRandomBoxEffect();
        }

    }
}