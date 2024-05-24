using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    private Dictionary<Renderer, Material[]> originalMaterials =
        new Dictionary<Renderer, Material[]>();
    private Material highlightRed;
    private Material highlightGreen;

    void Start()
    {
        highlightRed = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        highlightRed.color = Color.red;
        highlightGreen = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        highlightGreen.color = Color.green;

        Renderer[] objectRenderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            Material[] ogMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                ogMaterials[i] = new Material(renderer.materials[i]);
            }
            this.originalMaterials.Add(renderer, ogMaterials);
        }
    }

    public void HighlightRed()
    {
        Renderer[] objectRenderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            Material[] redMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                redMaterials[i] = highlightRed;
            }
            renderer.materials = redMaterials;
        }
    }

    public void HighlightGreen()
    {
        Renderer[] objectRenderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            Material[] greenMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                greenMaterials[i] = highlightGreen;
            }
        }
    }

    public void ReapplyOriginalColors()
    {
        Renderer[] objectRenderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in objectRenderers)
        {
            renderer.materials = originalMaterials[renderer];
        }
    }
}
