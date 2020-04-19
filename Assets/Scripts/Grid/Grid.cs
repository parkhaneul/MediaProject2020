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
    }
    public Grid(Vector3 center)
    {
        gridCenter = center;
    }
    public Grid()
    {
        gridCenter = Vector3.zero;
    }
}
