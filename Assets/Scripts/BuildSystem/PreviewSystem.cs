using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicatorPrefab;
    private List<GameObject> cellIndicators;
    public PlayerController playerController;

    private GameObject previewInstance;

    private Color happyColor = new Color(0, 1, 0, 0.5f);
    private Color sadColor = new Color(1, 0, 0, 0.5f);

    private ObjectData objectData;
    private Material previewMaterial;

    private void Start()
    {
        cellIndicators = new List<GameObject>();

        // Create a new transparent URP material
        previewMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        previewMaterial.SetFloat("_Surface", 1); // Transparent surface type
        previewMaterial.SetFloat("_Blend", 0); // Alpha blending
        previewMaterial.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        previewMaterial.SetFloat(
            "_DstBlend",
            (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha
        );
        previewMaterial.SetFloat("_ZWrite", 0);
        previewMaterial.EnableKeyword("_ALPHATEST_ON");
        previewMaterial.EnableKeyword("_ALPHABLEND_ON");
        previewMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        previewMaterial.renderQueue = 3000;
    }

    public void startPreview(ObjectData objectData)
    {
        // Clean up first
        stopPreview();
        this.objectData = objectData;
        this.previewInstance = objectData.getGameObject(new Vector3(0, 0, 0));
        TowerController tc = this.previewInstance.GetComponent<TowerController>();
        tc.setIsPreview();

        ApplyPreviewMaterial(previewInstance);
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
            ApplyPreviewMaterial(cellIndicator);
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
        for (int i = 0; i < cellIndicators.Count; i++)
        {
            GameObject cellIndicator = cellIndicators[i];
            cellIndicator.transform.position = new Vector3(
                occupiedCells[i].x + 0.5f,
                0,
                occupiedCells[i].y + 0.5f
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
        cellIndicators.Clear();
        objectData = null;
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
                Color color =
                    canBePlaced
                    && playerController.currency
                        >= previewInstance.GetComponent<TowerController>().cost
                        ? happyColor
                        : sadColor;
                material.color = color;
            }
        }
    }

    private void ApplyPreviewMaterial(GameObject gameObject)
    {
        Renderer[] objectRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                newMaterials[i] = new Material(previewMaterial);
            }
            renderer.materials = newMaterials;
        }
    }
}
