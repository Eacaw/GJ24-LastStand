using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int currency = 20; // Initial currency balance
    public UIDocument UIDocument;

    public int score = 0;

    private VisualElement root;
    private Label currencyCounter;
    private Label scoreCounter;
    private Label gameOverScoreLabel;

    void Start()
    {
        root = UIDocument.rootVisualElement;
        currencyCounter = root.Q<Label>("CurrencyCount");
        currencyCounter.text = currency.ToString();
        scoreCounter = root.Q<Label>("ScoreCount");
        gameOverScoreLabel = root.Q<Label>("GameOverScoreLabel");
        scoreCounter.text = score.ToString();
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

    public void AddScore(int amount)
    {
        score += amount;
        scoreCounter.text = score.ToString();
        gameOverScoreLabel.text = score.ToString();
    }
}
