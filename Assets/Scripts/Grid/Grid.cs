using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Placable owner { get; private set; }
    public List<Grid> adjacentGrids;
    public Vector3 gridCenter;
    public bool isOccupied;
    public Grid(Vector3 center, Placable owner, bool isOccupied)
    {
        gridCenter = center;
        this.owner = owner;
        this.isOccupied = isOccupied;
        adjacentGrids = new List<Grid>();
    }
    public Grid(Vector3 center, bool isOccupied)
    {
        gridCenter = center;
        this.isOccupied = isOccupied;
        adjacentGrids = new List<Grid>();
    }
    public Grid(Vector3 center)
    {
        owner = null;
        isOccupied = false;
        gridCenter = center;
        adjacentGrids = new List<Grid>();
    }
    public Grid()
    {
        owner = null;
        isOccupied = false;
        gridCenter = Vector3.one * Mathf.NegativeInfinity;
        adjacentGrids = new List<Grid>();
    }

    public void Occupy(Placable owner)
    {
        if(owner != null)
        {
            this.owner = owner;
            isOccupied = true;
        }
    }
    public void Unoccupy()
    {
        this.owner = null;
        isOccupied = false;
    }
}
