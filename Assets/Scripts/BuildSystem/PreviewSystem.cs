using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicatorPrefab;
    private List<GameObject> cellIndicators;

    private GameObject previewInstance;

    private Color happyColor = new Color(0, 1, 0, 0.5f);
    private Color sadColor = new Color(1, 0, 0, 0.5f);

    private ObjectData objectData;

    private void Start()
    {
        cellIndicators = new List<GameObject>();
    }

    public void startPreview(ObjectData objectData)
    {
        // Clean up first
        stopPreview();
        this.objectData = objectData;
        this.previewInstance = objectData.getGameObject(new Vector3(0, 0, 0));
        TowerController tc = this.previewInstance.GetComponent<TowerController>();
        tc.setIsPreview();

        prepareCellIndicator();
    }

    public void restartPreview(ObjectData objectData)
    {
        stopPreview();
        startPreview(objectData);
    }

    public void prepareCellIndicator()
    {
        foreach (Vector2Int cell in this.getOccupiedCells(Vector3Int.zero))
        {
            GameObject cellIndicator = Instantiate(
                cellIndicatorPrefab,
                new Vector3(0.5f + cell.x, 0, 0.5f + cell.y),
                Quaternion.identity
            );
            cellIndicators.Add(cellIndicator);
            UpdateRendererColor(cellIndicator, true);
        }
    }

    public void updatePreview(Vector3 worldCellCenter, Vector3Int gridPos, bool canBePlaced)
    {
        if (previewInstance == null)
        {
            return;
        }

        previewInstance.transform.position = worldCellCenter + new Vector3(0, previewYOffset, 0);
        UpdateRendererColor(previewInstance, canBePlaced);

        List<Vector2Int> occupiedCells = this.getOccupiedCells(gridPos);
        foreach (GameObject cellIndicator in cellIndicators)
        {
            int index = cellIndicators.IndexOf(cellIndicator);
            // TODO - Get rid of this magic number elegantly
            // TODO - Needs to be WorldCenterCell + given offset
            cellIndicator.transform.position = new Vector3(
                occupiedCells[index].x + 0.5f,
                0,
                occupiedCells[index].y + 0.5f
            );
            UpdateRendererColor(cellIndicator, canBePlaced);
        }
    }

    public void stopPreview()
    {
        // Stop preview, destroy all instances and reset variables
        Destroy(previewInstance);
        this.previewInstance = null;
        foreach (GameObject cellIndicator in cellIndicators)
        {
            Destroy(cellIndicator);
        }
        this.cellIndicators = new List<GameObject>();
        this.objectData = null;
    }

    public List<Vector2Int> getOccupiedCells(Vector3Int gridPos)
    {
        return objectData.getOccupiedCells(gridPos);
    }

    private void UpdateRendererColor(GameObject gameObject, bool canBePlaced)
    {
        Renderer[] objectRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = canBePlaced ? happyColor : sadColor;
            }
        }
    }
}
