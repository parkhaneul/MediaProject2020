using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBundle
{
    public Placable owner;
    public List<Grid> grids;

    public GridBundle(Grid source, Placable owner, List<Grid> searchTargets)
    {
        HashSet<Grid> touchingGrids = new HashSet<Grid>();
        SearchTouchingGrids(source, searchTargets, touchingGrids);

        this.owner = owner;
        grids = new List<Grid>();
        grids.Add(source);
        foreach(var grid in touchingGrids)
        {
            grids.Add(grid);
        }
    }

    private void SearchTouchingGrids(Grid target, List<Grid> targets, HashSet<Grid> collections)
    {
        targets.Remove(target);
        List<Grid> grids = GridManager.GetTouchingGrids(target, targets);
        int prevCount = collections.Count;
        foreach(var grid in grids)
        {
            collections.Add(grid);
        }
        if(grids.Count == prevCount)
        {
            return;
        }
        foreach(var grid in grids)
        {
            SearchTouchingGrids(grid, targets, collections);
        }
    } 

    public List<Grid> GetAdjacentGrids()
    {
        HashSet<Grid> adjacents = new HashSet<Grid>();
        foreach(var grid in grids)
        {
            foreach(var neighbor in grid.adjacentGrids)
            {
                adjacents.Add(neighbor);
            }
        }
        foreach(var grid in grids)
        {
            adjacents.Remove(grid);
        }
        return new List<Grid>(adjacents);
    }

    public List<Grid> GetVacantAdjacentGrids()
    {
        HashSet<Grid> adjacents = new HashSet<Grid>();
        foreach(var grid in grids)
        {
            foreach(var neighbor in grid.adjacentGrids)
            {
                if(!neighbor.isOccupied)
                    adjacents.Add(neighbor);
            }
        }
        foreach(var grid in grids)
        {
            adjacents.Remove(grid);
        }
        return new List<Grid>(adjacents);
    }
}
