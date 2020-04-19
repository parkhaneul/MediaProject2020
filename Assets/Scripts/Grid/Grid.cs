using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool isOccupied;
    public GameObject occupier;
    public List<Grid> adjacentGrids;
    public Vector3 gridCenter;

    public bool DEBUG_SELECTED = false;

    private Material material;
    
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        gameObject.transform.position = gridCenter;

        if(DEBUG_SELECTED)
        {
            foreach(var grid in adjacentGrids)
            {
                grid.transform.Rotate(new Vector3(0, Time.deltaTime * 10, 0));
            }   
        }

        if(isOccupied)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.cyan;
        }
    }
}
