/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3DObjects
{
    private Dictionary<Vector2Int, GameObject> visibleObjects;
    private List<Vector2Int> highlightedCells;

    public Grid3DObjects()
    {
        this.visibleObjects = new Dictionary<Vector2Int, GameObject>();
    }

    public bool addObject(Vector2Int gridPos, List<Vector2Int> occupiedCells, GameObject gameObject)
    {
        foreach (Vector2Int cell in occupiedCells)
        {
            if (this.visibleObjects.ContainsKey(cell))
            {
                return false;
            }
        }

        this.visibleObjects.Add(gridPos, gameObject);
        return true;
    }

    public List<GameObject> removeObject(List<Vector2Int> occupiedCells)
    {
        List<GameObject> removedObjects = new List<GameObject>();
        foreach (Vector2Int cell in occupiedCells)
        {
            if (this.visibleObjects.ContainsKey(cell))
            {
                removedObjects.Add(this.visibleObjects[cell]);
                this.visibleObjects.Remove(cell);
            }
        }

        return removedObjects;
    }

    public void setHighlightedCells(List<Vector2Int> highlightedCells)
    {
        foreach (Vector2Int cell in this.visibleObjects.Keys)
        {
            if (highlightedCells.Contains(cell))
            {
                this.visibleObjects[cell].gameObject
                    .GetComponentInChildren<ObjectHighlighter>()
                    .HighlightRed();
            }
            else
            {
                if (this.visibleObjects.ContainsKey(cell))
                {
                    this.visibleObjects[cell].gameObject
                        .GetComponentInChildren<ObjectHighlighter>()
                        .ReapplyOriginalColors();
                }
            }
            this.highlightedCells = highlightedCells;
        }
    }

    public void clearHighlightedCells()
    {
        foreach (Vector2Int cell in this.visibleObjects.Keys)
        {
            if (this.visibleObjects.ContainsKey(cell))
            {
                this.visibleObjects[cell].gameObject
                    .GetComponentInChildren<ObjectHighlighter>()
                    .ReapplyOriginalColors();
            }
        }
        this.highlightedCells = new List<Vector2Int>();
    }
}
