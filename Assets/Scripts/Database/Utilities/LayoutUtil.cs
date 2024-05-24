/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayoutUtil
{
    public static List<Vector2Int> getOccupiedCells(Vector3Int gridPos, LayoutData shape)
    {
        List<Vector2Int> occupiedCells = new List<Vector2Int>();
        foreach (Vector3Int cell in shape.Layout)
        {
            occupiedCells.Add(new Vector2Int(cell.x + gridPos.x, cell.z + gridPos.z));
        }
        return occupiedCells;
    }
}
