using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpawnerManager : MonoBehaviour
{
    private static ToolSpawnerManager instance;
    private Dictionary<ToolKind, ToolSpawner> spawners;
    public static ToolSpawnerManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
        else 
        {
            DestroyImmediate(this);
        }
    }
    
    void Start()
    {
        spawners = new Dictionary<ToolKind, ToolSpawner>();
        ToolSpawner[] scripts = FindObjectsOfType<ToolSpawner>();    

        for(int i = 0; i < scripts.Length; i++)
        {
            spawners.Add(scripts[i].kindOfTool, scripts[i]);
        }
    }
    public void SpawnTool(ToolKind kind)
    {
        spawners[kind].InstantiateTool();
    }
}
