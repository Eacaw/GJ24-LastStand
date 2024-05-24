using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPreview : MonoBehaviour
{
    public Vector2Int gridSize;

    public void setGridSize(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
    }

    public void Update()
    {
        transform.localScale = new Vector3(this.gridSize.x / 10, 1, this.gridSize.y / 10);
        transform.position = new Vector3(0, 0.015f, 0);
    }
}
