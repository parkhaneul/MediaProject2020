using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpawner : MonoBehaviour 
{
    public GameObject toolObject;
    public Vector3 offset;
    public ToolKind kindOfTool;

    public void InstantiateTool()
    {
        Instantiate(toolObject, transform.position + offset, Quaternion.identity);
    }
}
