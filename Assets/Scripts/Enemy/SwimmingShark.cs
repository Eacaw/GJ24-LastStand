using UnityEngine;

public class SwimmingShark : MonoBehaviour
{
    public PlayerController playerController;

    private void OnMouseDown()
    {
        playerController.AddCurrency(100);
    }
}
