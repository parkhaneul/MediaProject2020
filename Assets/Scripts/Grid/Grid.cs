using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Placable owner;
    public List<Grid> adjacentGrids;
    public Vector3 gridCenter;
    public bool isOccupied;
    public Grid(Vector3 center, bool isOccupied)
    {
        gridCenter = center;
        this.isOccupied = isOccupied;
        adjacentGrids = new List<Grid>();
    }
    public Grid(Vector3 center)
    {
        gridCenter = center;
        adjacentGrids = new List<Grid>();
    }
    public Grid()
    {
        gridCenter = Vector3.one * Mathf.NegativeInfinity;
        adjacentGrids = new List<Grid>();
    }
}
