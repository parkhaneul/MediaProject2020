using System.Collections;
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
    public int widthCount = 100;
    public int heightCount = 100;
    public bool debugMode = true;
    public bool isPlane = true;
    public GameObject gridGround;
    private Grid[,] gridArray;
    private List<GridBundle> bundles;
    private const int gridLayerMask = 1 << 8; 
    private const int planeOffset = 10; //Plane consists of 10 quads.
    static private float tileWidth;
    static private float tileHeight;
    static private float planeWidth;
    static private float planeHeight;
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
            for(int i = 0 ; i < widthCount ; i++)
            {
                for(int j = 0 ; j < heightCount ; j++)
                {
                    Grid grid = gridArray[i,j];
                    Debug.DrawLine(grid.gridCenter, grid.gridCenter + Vector3.up, 
                        grid.isOccupied ? Color.red : Color.blue);
                }
            }
        }
    }

    public void GenerateGrids()
    {
        Clear();

        tileWidth = (isPlane ? planeOffset : 1) * gridGround.transform.localScale.x / widthCount;
        tileHeight = (isPlane ? planeOffset : 1) * gridGround.transform.localScale.z / heightCount;
        planeWidth = (isPlane ? planeOffset : 1) * gridGround.transform.localScale.x;
        planeHeight = (isPlane ? planeOffset : 1) * gridGround.transform.localScale.z;

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

    public GridBundle GetGridBundles(Placable placable)
    {
        foreach (var bundle in bundles)
        {
            if (bundle.owner == placable)
            {
                return bundle;
            }
        }
        return null;
    }

    public Grid GetNeighborGridFromDirection(Placable placable, Vector3 dir)
    {
        foreach(var bundle in bundles)
        {
            if(bundle.owner == placable)
            {
                if(bundle.grids.Count > 1)
                {
                    Debug.LogError("This method should be called from placable object which has only one grid");
                    return null;
                }

                return bundle.GetGridFromDirection(dir);
            }
        }
        return null;
    }

    public Grid[] GetNeighborGridsFromDirection(Placable placable, Vector3 dir)
    {
        foreach (var bundle in bundles)
        {
            if (bundle.owner == placable)
            {
                return bundle.GetGridsFromDirection(dir);
            }
        }
        return null;
    }

    public void Move(Placable placable, Grid grid)
    {
        foreach (var bundle in bundles)
        {
            if (bundle.owner == placable)
            {
                if (bundle.grids.Count > 1)
                {
                    Debug.LogError("This method should be called from placable object which has only one grid");
                    return;
                }
                bundle.MoveToNewGrid(grid);
            }
        }
    }

    public void Move(Placable placable, Grid[] grids)
    {
        foreach (var bundle in bundles)
        {
            if (bundle.owner == placable)
            {
                if (bundle.grids.Count != grids.Length)
                {
                    Debug.LogError("Spaces of destination and origin should be same.");
                    return;
                }
                bundle.MoveToNewGrids(grids);
            }
        }
    }

    public void UnoccupyPlacable(Placable placable)
    {
        GridBundle target = null;
        foreach (var bundle in bundles)
        {
            if (bundle.owner == placable)
            {
                target = bundle;
                bundle.owner = null;
                foreach(var grid in bundle.grids)
                {
                    grid.Unoccupy();
                }
                break;
            }
        }
        if(target != null)
        {
            bundles.Remove(target);
        }
    }

    private void Clear()
    {
        tileWidth = -1.0f;
        tileHeight = -1.0f;
        planeWidth = -1.0f;
        gridArray = null;
        bundles.Clear();
    }
    private List<Grid> InitGrid()
    {
        List<Grid> occupiedGrids = new List<Grid>();
        List<Vector3>  gridCenters = new List<Vector3>();
        gridArray = new Grid[widthCount,heightCount];
        for (int i = 0; i < widthCount; i++)
        {
            for (int j = 0; j < heightCount; j++)
            {
                Vector3 center = gridGround.transform.position
                    - Vector3.forward * j * tileHeight + Vector3.forward * planeHeight / 2
                    - Vector3.right * i * tileWidth + Vector3.right * planeWidth / 2
                    - Vector3.right * tileWidth / 2 - Vector3.forward * tileHeight / 2;

                if (!isPlane)
                    center.y += gridGround.transform.lossyScale.y * 0.5f;
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
        for (int i = 0; i < widthCount; i++)
        {
            for (int j = 0; j < heightCount; j++)
            {
                for(int k = -1; k < 2; k++)
                {
                    for(int l = -1; l < 2; l++)
                    {
                        if( (k != 0 || l != 0) && (i+k >= 0 && i+k <widthCount) && (j+l >= 0 && j+l <heightCount) )
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
            GridBundle bundle = new GridBundle(target, target.owner, occupiedGrids);
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
        float biggerOne = tileHeight > tileWidth ? tileHeight : tileWidth;
        return (a.gridCenter - b.gridCenter).sqrMagnitude <= biggerOne * biggerOne ? true : false;
    }

    ///<summary>
    /// Up, Down, Right, Left, Diagonal
    ///</summary>
    public static bool isGridAdjacentWith(Grid a, Grid b)
    {
        return (a.gridCenter - b.gridCenter).sqrMagnitude <= tileWidth * tileWidth + tileHeight * tileHeight? true : false;
    }
}


