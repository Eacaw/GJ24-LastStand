using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Transform target;
    public Vector3 offset;

    private Camera mainCamera;
    private float originalWidth;

    void Start()
    {
        mainCamera = Camera.main;
        originalWidth = healthBar.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        // Update the position of the health bar to follow the target
        transform.position = target.position + offset;
        transform.rotation = mainCamera.transform.rotation;
    }

    public void SetHealth(float healthPercentage)
    {
        Vector2 sizeDelta = healthBar.rectTransform.sizeDelta;
        sizeDelta.x = originalWidth * healthPercentage;
        healthBar.rectTransform.sizeDelta = sizeDelta;
    }
}
