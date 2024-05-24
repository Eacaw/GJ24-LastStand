/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    private Grid3DObjects grid3DObjects;
    private Grid grid;

    [SerializeField]
    private Vector2Int gridSize;

    [SerializeField]
    private GameObject gridPreviewPrefab;
    private GameObject gridPreview;

    [SerializeField]
    private ObjectDatabaseController objectDatabaseController;

    [SerializeField]
    private PreviewSystem previewSystem;

    [SerializeField]
    private InputManager inputManager;

    public Dictionary<Vector2Int, bool> gridOccupied = new Dictionary<Vector2Int, bool>();
    public Dictionary<Vector2Int, string> objectIdMap = new Dictionary<Vector2Int, string>();

    [SerializeField]
    public bool inBuildMode = false;
    public bool inDeleteMode = false;
    private String currentObjectId;

    private void Start()
    {
        grid3DObjects = new Grid3DObjects();
        this.grid = GetComponent<Grid>();
        this.gridPreview = Instantiate(
            gridPreviewPrefab,
            new Vector3(0, 0, 0),
            Quaternion.identity
        );
        this.gridPreview.GetComponent<GridPreview>().setGridSize(this.gridSize);
        this.inBuildMode = false;
        this.inDeleteMode = false;

        // Setup the input manager event actions
        inputManager.OnLmb += AddObject;
        inputManager.OnR += OnRotate;
        inputManager.OnEsc += endBuildMode;
    }

    public void Update()
    {
        if (inBuildMode)
        {
            Vector3 worldPos = inputManager.getMouseWorldPosition();
            Vector3Int gridPos = GridPositionUtil.getGridFromWorld(worldPos, this.grid);
            Vector3 worldCellCenter = GridPositionUtil.getWorldCellCenterFromWorld(
                worldPos,
                this.grid
            );
            this.previewSystem.updatePreview(
                worldCellCenter,
                gridPos,
                canPlaceObject(
                    GridPositionUtil.get2DGridFromWorld(worldPos, this.grid),
                    this.previewSystem.getOccupiedCells(gridPos)
                )
            );
        }
        else if (inDeleteMode)
        {
            Vector3 worldPos = inputManager.getMouseWorldPosition();
            Vector3Int gridPos = GridPositionUtil.getGridFromWorld(worldPos, this.grid);
            Vector2Int gridPos2D = GridPositionUtil.get2DGridFromWorld(worldPos, this.grid);

            if (this.objectIdMap.ContainsKey(gridPos2D))
            {
                String objectId = this.objectIdMap[gridPos2D];
                ObjectData objectData = objectDatabaseController.GetObjectData(objectId);
                List<Vector2Int> occupiedCells = objectData.getOccupiedCells(
                    GridPositionUtil.getGridFrom2DGrid(gridPos2D)
                );
                this.grid3DObjects.setHighlightedCells(occupiedCells);
            }
            else
            {
                this.grid3DObjects.clearHighlightedCells();
            }
        }
    }

    public void AddObject()
    {
        if (inBuildMode)
        {
            Vector3 worldPos = inputManager.getMouseWorldPosition();
            Vector2Int gridPos = GridPositionUtil.get2DGridFromWorld(worldPos, this.grid);
            ObjectData objectData = objectDatabaseController.GetObjectData(this.currentObjectId);
            List<Vector2Int> occupiedCells = objectData.getOccupiedCells(
                GridPositionUtil.getGridFrom2DGrid(gridPos)
            );
            if (!canPlaceObject(gridPos, occupiedCells))
            {
                return;
            }
            Vector3 worldCellCenter = GridPositionUtil.getWorldCellCenterFrom2DGrid(
                gridPos,
                this.grid
            );

            GameObject gameObject = objectData.getGameObject(worldCellCenter);
            bool objectPlaced = this.grid3DObjects.addObject(gridPos, occupiedCells, gameObject);
            this.objectIdMap.Add(gridPos, this.currentObjectId);
            foreach (Vector2Int cell in occupiedCells)
            {
                gridOccupied.Add(cell, true);
            }

            if (!objectPlaced)
            {
                Destroy(gameObject);
                this.objectIdMap.Remove(gridPos);
                foreach (Vector2Int cell in occupiedCells)
                {
                    gridOccupied.Remove(cell);
                }
            }
        }
    }

    private void OnRotate()
    {
        if (inBuildMode)
        {
            ObjectData objectData = objectDatabaseController.GetObjectData(this.currentObjectId);
            objectData.rotate();
            this.previewSystem.restartPreview(objectData);
        }
    }

    public void startBuildMode(String objectId)
    {
        endDeleteMode();
        gridPreview.SetActive(true);
        this.previewSystem.startPreview(objectDatabaseController.GetObjectData(objectId));
        this.currentObjectId = objectId;
        this.inBuildMode = true;
        this.inDeleteMode = false;
    }

    public void endBuildMode()
    {
        this.inBuildMode = false;
        this.previewSystem.stopPreview();
        this.currentObjectId = null;
        gridPreview.SetActive(false);
    }

    public void startDeleteMode()
    {
        endBuildMode();
        gridPreview.SetActive(true);
        this.inDeleteMode = true;
        this.inBuildMode = false;
        inputManager.OnLmb -= AddObject;
        inputManager.OnLmb += removeObject;
    }

    public void endDeleteMode()
    {
        this.inDeleteMode = false;
        inputManager.OnLmb -= removeObject;
        inputManager.OnLmb += AddObject;
        this.grid3DObjects.clearHighlightedCells();
        gridPreview.SetActive(false);
    }

    private void removeObject()
    {
        Vector3 worldPos = inputManager.getMouseWorldPosition();
        Vector2Int gridPos = GridPositionUtil.get2DGridFromWorld(worldPos, this.grid);

        if (this.objectIdMap.ContainsKey(gridPos))
        {
            String objectId = this.objectIdMap[gridPos];
            ObjectData objectData = objectDatabaseController.GetObjectData(objectId);
            List<Vector2Int> occupiedCells = objectData.getOccupiedCells(
                GridPositionUtil.getGridFrom2DGrid(gridPos)
            );
            List<GameObject> removedObjects = this.grid3DObjects.removeObject(occupiedCells);
            this.objectIdMap.Remove(gridPos);

            foreach (GameObject gameObject in removedObjects)
            {
                Destroy(gameObject);
            }

            foreach (Vector2Int cell in occupiedCells)
            {
                this.gridOccupied.Remove(cell);
            }
        }
    }

    private bool canPlaceObject(Vector2Int gridPosition, List<Vector2Int> occupiedCells)
    {
        foreach (Vector2Int cell in occupiedCells)
        {
            if (gridOccupied.ContainsKey(cell))
            {
                return false;
            }
        }
        return true;
    }
}
