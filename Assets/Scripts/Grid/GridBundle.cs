using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBundle
{
    public Placable owner;
    public List<Grid> grids;

    public GridBundle(Grid source, List<Grid> searchTargets)
    {
        SearchTouchingGrids(source, searchTargets, grids);
        grids.Add(source);
    }

    private void SearchTouchingGrids(Grid target, List<Grid> targets, List<Grid> collections)
    {
        List<Grid> grids = GridManager.GetTouchingGrids(target, targets);
        if(grids.Count == 0)
        {
            return;
        }

        foreach(var grid in grids)
        {
            SearchTouchingGrids(grid, targets, collections);
        }

        collections.AddRange(grids);
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
