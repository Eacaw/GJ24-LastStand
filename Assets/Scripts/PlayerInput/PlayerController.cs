using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int currency = 20;  // Initial currency balance

    public void AddCurrency(int amount)
    {
        currency += amount;
        Debug.Log("Currency added: " + amount + ". New balance: " + currency);
    }

    public bool SubtractCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            Debug.Log("Currency subtracted: " + amount + ". New balance: " + currency);
            return true;
        }
        else
        {
            Debug.Log("Not enough currency. Current balance: " + currency);
            return false;
        }
    }
}
