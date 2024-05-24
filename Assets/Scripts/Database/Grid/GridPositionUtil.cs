using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridPositionUtil
{
    public static Vector2Int get2DGridFromWorld(Vector3 worldPos, Grid grid)
    {
        Vector3Int gridPos = getGridFromWorld(worldPos, grid);
        return new Vector2Int(gridPos.x, gridPos.z);
    }

    public static Vector2Int get2DGridFromGrid(Vector3Int gridPos)
    {
        return new Vector2Int(gridPos.x, gridPos.z);
    }

    public static Vector3Int getGridFromWorld(Vector3 worldPos, Grid grid)
    {
        return grid.WorldToCell(worldPos);
    }

    public static Vector3Int getGridFrom2DGrid(Vector2Int gridPos)
    {
        return new Vector3Int(gridPos.x, 0, gridPos.y);
    }

    public static Vector3 getWorldFromGrid(Vector3Int gridPos, Grid grid)
    {
        return grid.CellToWorld(gridPos);
    }

    public static Vector3 getWorldFrom2DGrid(Vector2Int gridPos, Grid grid)
    {
        return getWorldFromGrid(new Vector3Int((int)gridPos.x, 0, (int)gridPos.y), grid);
    }

    public static Vector3 getWorldCellCenter(Vector3Int gridPos, Grid grid)
    {
        Vector3 cellCenter = grid.GetCellCenterWorld(gridPos);
        return new Vector3(cellCenter.x, 0, cellCenter.z);
    }

    public static Vector3 getWorldCellCenterFrom2DGrid(Vector2Int gridPos, Grid grid)
    {
        return getWorldCellCenter(new Vector3Int((int)gridPos.x, 0, (int)gridPos.y), grid);
    }

    public static Vector3 getWorldCellCenterFromWorld(Vector3 worldPos, Grid grid)
    {
        Vector3Int gridPos = getGridFromWorld(worldPos, grid);
        return getWorldCellCenter(gridPos, grid);
    }
}
