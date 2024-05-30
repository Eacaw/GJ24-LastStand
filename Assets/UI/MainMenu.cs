using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = root.Q<Button>("startButton");
        startButton.clicked += () => StartGame();

        Button instructionsButton = root.Q<Button>("instructionsButton");
        instructionsButton.clicked += () => OpenInstructionsPanel();

        Button closeButton = root.Q<Button>("closeButton");
        closeButton.clicked += () => CloseInstructionsPanel();
    }

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement instructionsPanel = root.Q<VisualElement>("instructionsPanel");
        instructionsPanel.style.display = DisplayStyle.None;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PirateIsland");
    }

    public void OpenInstructionsPanel()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement instructionsPanel = root.Q<VisualElement>("instructionsPanel");
        instructionsPanel.style.display = DisplayStyle.Flex;
    }

    public void CloseInstructionsPanel()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement instructionsPanel = root.Q<VisualElement>("instructionsPanel");
        instructionsPanel.style.display = DisplayStyle.None;
    }
}
