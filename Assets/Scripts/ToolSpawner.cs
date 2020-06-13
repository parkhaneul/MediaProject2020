using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpawner : MonoBehaviour 
{
    public GameObject toolObject;
    public Vector3 offset;
    public ToolKind kindOfTool;

    public void InstantiateTool(GameObject tool)
    {
        tool.transform.position = transform.position + offset;
        tool.transform.rotation = Quaternion.identity;
    }
}
