﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour //TODO : Make This SingleTon
{
    //
    // This component should be attached to 'ground' like mesh (for example, plane)
    //
    //
    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    public int tileCount = 100;
    public bool debugMode = true;
    public GameObject gridGround;
    private Grid[,] gridArray;
    private List<GridBundle> bundles;
    private const int gridLayerMask = 1 << 8; 
    private const int planeOffset = 10; //Plane consists of 10 quads.
    static private float tileWidth;
    static private float planeWidth;
    private const float rootTwo = 1.414f;

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
        bundles = new List<GridBundle>();
        GenerateGrids();
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode)
        {
            // foreach(var center in gridCenters)
            // {
            //     // Debug.DrawLine(center, center + Vector3.up, Color.red);
            // }
        }
    }

    public void GenerateGrids()
    {
        Clear();

        tileWidth = planeOffset * gridGround.transform.localScale.x / tileCount;
        planeWidth = planeOffset * gridGround.transform.localScale.x;

        List<Grid> occupiedGrids = InitGrid();
        InitAdjacent();
        InitBundles(occupiedGrids);
    }

    public Grid GetRandomItemSpawnPosition(Placable placable)
    {
        foreach(var bundle in bundles)
        {
            if(bundle.owner == placable)
            {
                List<Grid> candidates = bundle.GetVacantAdjacentGrids();
                return candidates[Random.Range(0, candidates.Count)];
            }
        }
        return new Grid();
    }
    private void Clear()
    {
        tileWidth = -1.0f;
        planeWidth = -1.0f;
        gridArray = null;
        bundles.Clear();
    }
    private List<Grid> InitGrid()
    {
        List<Grid> occupiedGrids = new List<Grid>();
        List<Vector3>  gridCenters = new List<Vector3>();
        gridArray = new Grid[tileCount,tileCount];
        for (int i = 0; i < tileCount; i++)
        {
            for (int j = 0; j < tileCount; j++)
            {
                Vector3 center = gridGround.transform.position
                    - Vector3.forward * j * tileWidth + Vector3.forward * planeWidth / 2
                    - Vector3.right * i * tileWidth + Vector3.right * planeWidth / 2
                    - Vector3.right * tileWidth / 2 - Vector3.forward * tileWidth / 2;
                gridCenters.Add(center);
                
                Placable occupier;
                bool occupied = isGridOccupied(center, out occupier);
                
                Grid grid = new Grid(center, occupier, occupied);
                gridArray[i,j] = grid;

                if(grid.isOccupied)
                    occupiedGrids.Add(grid);
            }
        }

        return occupiedGrids;
    }
    private void InitAdjacent()
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

    private void InitBundles(List<Grid> occupiedGrids)
    {
        while(occupiedGrids.Count > 0)
        {
            Grid target = occupiedGrids[0];
            GridBundle bundle = new GridBundle(target,target.owner, occupiedGrids);
            bundles.Add(bundle);
        }
    }

    public static List<Grid> GetTouchingGrids(Grid target, List<Grid> grids) //TODO : Use Linq instead of foreach
    {
        List<Grid> neighbors = new List<Grid>();
        foreach(var grid in grids)
        {
            if(isGridTouchingWith(target, grid))
                neighbors.Add(grid);
        }
        return neighbors;
    }

    public static bool isGridOccupied(Vector3 center, out Placable placable, float length = Mathf.Infinity)
    {
        return isGridOccupied(center, Vector3.up, out placable, length);
    }

    public static bool isGridOccupied(Vector3 center, float length = Mathf.Infinity)
    {
        return isGridOccupied(center, Vector3.up, length);
    }

    public static bool isGridOccupied(Vector3 center, Vector3 direction, float length = Mathf.Infinity)
    {
        if(Physics.Raycast(center, direction, Mathf.Infinity, gridLayerMask))
        {
            //Instead of using layer Mask, we can check Placable interface.
            //Consider this later.
            return true;
        }
        return false;
    }
    public static bool isGridOccupied(Vector3 center, Vector3 direction, out Placable placable, float length = Mathf.Infinity)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(center, direction, out hitInfo, Mathf.Infinity, gridLayerMask))
        {
            Placable p = hitInfo.transform.gameObject.GetComponent<Placable>();
            placable = p;
            return true;
        }
        placable = null;
        return false;
    }

    ///<summary>
    /// Up, Down, Right, Left
    ///</summary>
    public static bool isGridTouchingWith(Grid a, Grid b)
    {
        return (a.gridCenter - b.gridCenter).sqrMagnitude <= tileWidth * tileWidth ? true : false;
    }

    ///<summary>
    /// Up, Down, Right, Left, Diagonal
    ///</summary>
    public static bool isGridAdjacentWith(Grid a, Grid b)
    {
        return (a.gridCenter - b.gridCenter).sqrMagnitude <= tileWidth * tileWidth * rootTwo ? true : false;
    }
}


