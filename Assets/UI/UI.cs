using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    // public PlacementSystem placementSystem;
    public GridData gridData;
    public ObjectDatabaseController objectDatabaseController;
    public CameraController cameraController;
    public Spawn spawncontroller;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button RedbeardButton = root.Q<Button>("Redbeard");
        string redbeardId = objectDatabaseController.GetObjectDataByName("Redbeard").Id;
        RedbeardButton.clicked += () => gridData.startBuildMode(redbeardId);

        Button LongJohnButton = root.Q<Button>("LongJohn");
        string longJohnId = objectDatabaseController.GetObjectDataByName("LongJohn").Id;
        LongJohnButton.clicked += () => gridData.startBuildMode(longJohnId);

        Button IronMaryButton = root.Q<Button>("IronMary");
        string ironMaryId = objectDatabaseController.GetObjectDataByName("IronMary").Id;
        IronMaryButton.clicked += () => gridData.startBuildMode(ironMaryId);

        Button BarricadeButton = root.Q<Button>("Barricade");
        string barricadeId = objectDatabaseController.GetObjectDataByName("Barricade").Id;
        BarricadeButton.clicked += () => gridData.startBuildMode(barricadeId);

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
        };

        VisualElement upgradeTab = root.Q<VisualElement>("UpgradeTab");
        Button closeButton = upgradeTab.Q<Button>("CloseButton");
        closeButton.clicked += () => upgradeTab.style.display = DisplayStyle.None;
        upgradeTab.style.display = DisplayStyle.None;
    }
}
