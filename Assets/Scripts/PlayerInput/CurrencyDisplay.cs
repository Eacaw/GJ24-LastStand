using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText;  // Reference to the TextMeshPro component
    private PlayerController playerController;

    void Start()
    {
        // Find the PlayerController in the scene
        GameObject playerControllerObject = GameObject.Find("PlayerController");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found. Please ensure there is a PlayerController in the scene.");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            // Update the currency text with the player's current currency
            currencyText.text = "Currency: " + playerController.currency.ToString();
        }
    }
}
