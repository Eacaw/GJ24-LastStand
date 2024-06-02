using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // public PlacementSystem placementSystem;
    public GridData gridData;
    public ObjectDatabaseController objectDatabaseController;
    public CameraController cameraController;
    public Spawn spawncontroller;
    public Texture barricadeImage;
    public Texture redbeardImage;
    public Texture longJohnImage;
    public Texture ironMaryImage;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement TooltipPanel = root.Q<VisualElement>("TooltipPanel");
        TooltipPanel.style.display = DisplayStyle.None;

        VisualElement mainMenuPanel = root.Q<VisualElement>("MenuPanel");
        mainMenuPanel.style.display = DisplayStyle.None;

        Button RedbeardButton = root.Q<Button>("Redbeard");
        string redbeardId = objectDatabaseController.GetObjectDataByName("Redbeard").Id;
        RedbeardButton.clicked += () =>
        {
            gridData.startBuildMode(redbeardId);
            TooltipPanel.style.display = DisplayStyle.Flex;
            TooltipPanel.Q<Label>("tooltipTitle").text = "Redbeard";
            TooltipPanel.Q<Label>("tooltipHealth").text = "Health - 100";
            TooltipPanel.Q<Label>("tooltipDamage").text = "Damage - 20";
            TooltipPanel.Q<Label>("tooltipRange").text = "Range - 3";
            TooltipPanel.Q<Label>("tooltipCost").text = "Cost - 10";
            TooltipPanel.Q<VisualElement>("tooltipImage").style.backgroundImage =
                (StyleBackground)redbeardImage;
        };

        Button LongJohnButton = root.Q<Button>("LongJohn");
        string longJohnId = objectDatabaseController.GetObjectDataByName("LongJohn").Id;
        LongJohnButton.clicked += () =>
        {
            gridData.startBuildMode(longJohnId);
            TooltipPanel.style.display = DisplayStyle.Flex;
            TooltipPanel.Q<Label>("tooltipTitle").text = "Long John";
            TooltipPanel.Q<Label>("tooltipHealth").text = "Health - 75";
            TooltipPanel.Q<Label>("tooltipDamage").text = "Damage - 15";
            TooltipPanel.Q<Label>("tooltipRange").text = "Range - 4";
            TooltipPanel.Q<Label>("tooltipCost").text = "Cost - 20";
            TooltipPanel.Q<VisualElement>("tooltipImage").style.backgroundImage =
                (StyleBackground)longJohnImage;
        };

        Button IronMaryButton = root.Q<Button>("IronMary");
        string ironMaryId = objectDatabaseController.GetObjectDataByName("IronMary").Id;
        IronMaryButton.clicked += () =>
        {
            gridData.startBuildMode(ironMaryId);
            TooltipPanel.style.display = DisplayStyle.Flex;
            TooltipPanel.Q<Label>("tooltipTitle").text = "Iron Mary";
            TooltipPanel.Q<Label>("tooltipHealth").text = "Health - 95";
            TooltipPanel.Q<Label>("tooltipDamage").text = "Damage - 25";
            TooltipPanel.Q<Label>("tooltipRange").text = "Range - 5";
            TooltipPanel.Q<Label>("tooltipCost").text = "Cost - 25";
            TooltipPanel.Q<VisualElement>("tooltipImage").style.backgroundImage =
                (StyleBackground)ironMaryImage;
        };

        Button BarricadeButton = root.Q<Button>("Barricade");
        string barricadeId = objectDatabaseController.GetObjectDataByName("Barricade").Id;
        BarricadeButton.clicked += () =>
        {
            gridData.startBuildMode(barricadeId);
            TooltipPanel.style.display = DisplayStyle.Flex;
            TooltipPanel.Q<Label>("tooltipTitle").text = "Barricade";
            TooltipPanel.Q<Label>("tooltipHealth").text = "Health - 100";
            TooltipPanel.Q<Label>("tooltipDamage").text = "Damage - 0";
            TooltipPanel.Q<Label>("tooltipRange").text = "Range - 0";
            TooltipPanel.Q<Label>("tooltipCost").text = "Cost - 5";
            TooltipPanel.Q<VisualElement>("tooltipImage").style.backgroundImage =
                (StyleBackground)barricadeImage;
        };

        Button RotateLeft = root.Q<Button>("RotateLeft");
        RotateLeft.clicked += () => cameraController.RotateCameraLeft();

        Button RotateRight = root.Q<Button>("RotateRight");
        RotateRight.clicked += () => cameraController.RotateCameraRight();

        Button startButton = root.Q<Button>("startButton");
        startButton.clicked += () =>
        {
            spawncontroller.canStart = true;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyMovement>().canStart = true;
            }
            gridData.endBuildMode();
            startButton.style.display = DisplayStyle.None;
        };

        VisualElement upgradeTab = root.Q<VisualElement>("UpgradeTab");
        Button closeButton = upgradeTab.Q<Button>("CloseButton");
        closeButton.clicked += () => upgradeTab.style.display = DisplayStyle.None;
        upgradeTab.style.display = DisplayStyle.None;

        closeGameOverPanel();

        Button GameOverMenuButton = root.Q<Button>("GameOverMenuButton");
        GameOverMenuButton.clicked += () => SceneManager.LoadScene("StartScene");

        Button MenuButton = root.Q<Button>("MenuButton");
        MenuButton.clicked += () => openMainMenuPanel();

        Button MenuCloseButton = root.Q<Button>("MenuCloseButton");
        MenuCloseButton.clicked += () => closeMainMenuPanel();

        Button RestartMenuButton = root.Q<Button>("RestartMenuButton");
        RestartMenuButton.clicked += () =>
        {
            SceneManager.LoadScene("PirateIsland");
            Time.timeScale = 1;
        };

        Button MainMenuButton = root.Q<Button>("MainMenuButton");
        MainMenuButton.clicked += () =>
        {
            SceneManager.LoadScene("StartScene");
            Time.timeScale = 1;
        };
    }

    public void openMainMenuPanel()
    {
        Time.timeScale = 0;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement mainMenuPanel = root.Q<VisualElement>("MenuPanel");
        mainMenuPanel.style.display = DisplayStyle.Flex;
    }

    public void closeMainMenuPanel()
    {
        Time.timeScale = 1;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement mainMenuPanel = root.Q<VisualElement>("MenuPanel");
        mainMenuPanel.style.display = DisplayStyle.None;
    }

    public void openGameOverPanel()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOverPanel = root.Q<VisualElement>("GameOverPanel");
        gameOverPanel.style.display = DisplayStyle.Flex;
    }

    private void closeGameOverPanel()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOverPanel = root.Q<VisualElement>("GameOverPanel");
        gameOverPanel.style.display = DisplayStyle.None;
    }
}
