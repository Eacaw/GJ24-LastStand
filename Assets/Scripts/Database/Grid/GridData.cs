/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Dictionary<Vector2Int, bool> gridPathOccupied = new Dictionary<Vector2Int, bool>();
    public Dictionary<Vector2Int, string> objectIdMap = new Dictionary<Vector2Int, string>();

    [SerializeField]
    public bool inBuildMode = false;
    private String currentObjectId;

    public UIDocument UIDocument;

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
            TowerController tc = gameObject.GetComponent<TowerController>();
            bool hasEnoughCurrency = tc.TowerPlaced();
            if (!hasEnoughCurrency)
            {
                return;
            }
            bool objectPlaced = this.grid3DObjects.addObject(gridPos, occupiedCells, gameObject);
            this.objectIdMap.Add(gridPos, this.currentObjectId);
            foreach (Vector2Int cell in occupiedCells)
            {
                gridOccupied.Add(cell, true);
                if (objectData.occupiesCells)
                {
                    gridPathOccupied.Add(cell, true);
                }
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
        else
        {
            Vector3 worldPos = inputManager.getMouseWorldPosition();
            Vector2Int gridPos = GridPositionUtil.get2DGridFromWorld(worldPos, this.grid);
            if (this.objectIdMap.ContainsKey(gridPos))
            {
                String objectId = this.objectIdMap[gridPos];
                ObjectData objectData = objectDatabaseController.GetObjectData(objectId);
                TowerController tc = objectData.Prefab.GetComponent<TowerController>();

                VisualElement root = UIDocument.rootVisualElement;
                VisualElement upgradeTab = root.Q<VisualElement>("UpgradeTab");
                tc.isSelected(upgradeTab);

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
        gridPreview.SetActive(true);
        this.previewSystem.startPreview(objectDatabaseController.GetObjectData(objectId));
        this.currentObjectId = objectId;
        this.inBuildMode = true;
    }

    public void endBuildMode()
    {
        this.inBuildMode = false;
        this.previewSystem.stopPreview();
        this.currentObjectId = null;
        // This is set to false so that we can get clicks mid game
        // gridPreview.SetActive(false);
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

    public bool isPositionOccupied(Vector2Int gridPosition)
    {
        bool result = false;
        return gridPathOccupied.TryGetValue(gridPosition, out result);
    }
}
