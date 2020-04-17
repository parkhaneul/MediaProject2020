using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour //TODO : Make This SingleTon
{
    //
    // This component should be attached to 'ground' like mesh (for example, plane)
    //
    //

    public int tileCount = 100;
    public bool debugMode = true;
    public GameObject gridGround;
    public GameObject DEBUG_grid;
    private Grid[,] gridArray;
    private List<Vector3> gridCenters;
    private const int planeOffset = 10; //Plane consists of 10 quads.

    void Start()
    {
        GenerateGrids();
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode)
        {
            foreach(var center in gridCenters)
            {
                // Debug.DrawLine(center, center + Vector3.up, Color.red);
            }
        }
    }

    public void GenerateGrids()
    {
        gridCenters = new List<Vector3>();
        float tileWidth = planeOffset * gridGround.transform.localScale.x / tileCount;
        float planeWidth = planeOffset * gridGround.transform.localScale.x;

        gridArray = new Grid[tileCount,tileCount];
        GameObject grids = new GameObject();
        grids.name = "Grids";
        for (int i = 0; i < tileCount; i++)
        {
            for (int j = 0; j < tileCount; j++)
            {
                Vector3 center = gridGround.transform.position
                    - Vector3.forward * j * tileWidth + Vector3.forward * planeWidth / 2
                    - Vector3.right * i * tileWidth + Vector3.right * planeWidth / 2
                    - Vector3.right * tileWidth / 2 - Vector3.forward * tileWidth / 2;
                gridCenters.Add(center);

                bool occupied = false;
                int layerMask = 1 << 8;

                if(Physics.Raycast(center, Vector3.up, Mathf.Infinity, layerMask))
                {
                    occupied = true;
                }

                GameObject grid = Instantiate(DEBUG_grid);
                Grid s_grid = grid.GetComponent<Grid>();
                Vector3 newCenter = new Vector3(center.x, center.y + 0.1f, center.z);
                s_grid.gridCenter = newCenter;
                s_grid.isOccupied = occupied;

                grid.transform.SetParent(grids.transform);
                gridArray[i,j] = s_grid;
            }
        }

        UpdateAdjacent();

    }
    
    private void UpdateAdjacent()
    {
        for (int i = 0; i < tileCount; i++)
        {
            for (int j = 0; j < tileCount; j++)
            {
                for(int k = -1; k < 2; k++)
                {
                    for(int l = -1; l < 2; l++)
                    {
                        if( (k != 0 && l != 0) && (i+k >= 0 && i+k <tileCount) && (j+l >= 0 && j+l <tileCount) )
                            gridArray[i,j].adjacentGrids.Add(gridArray[i+k, j+l]);
                    }
                }
            }
        }
    }
    public Vector3 GetClosestGridCenter()
    {
        return Vector3.zero;
    }
}
