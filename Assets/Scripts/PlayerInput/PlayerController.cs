using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int currency = 20; // Initial currency balance
    public UIDocument UIDocument;

    private VisualElement root;
    private Label currencyCounter;

    void Start()
    {
        root = UIDocument.rootVisualElement;
        currencyCounter = root.Q<Label>("CurrencyCount");
        currencyCounter.text = currency.ToString();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        currencyCounter.text = currency.ToString();
    }

    public bool SubtractCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            currencyCounter.text = currency.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }
}
