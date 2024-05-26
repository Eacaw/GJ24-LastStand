using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    // public PlacementSystem placementSystem;
    public GridData gridData;
    public ObjectDatabaseController objectDatabaseController;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button standButton = root.Q<Button>("standButton");
        string standId = objectDatabaseController.GetObjectDataByName("Stand").Id;
        standButton.clicked += () => gridData.startBuildMode(standId);

        Button chairButton = root.Q<Button>("chairButton");
        string chairId = objectDatabaseController.GetObjectDataByName("Chair").Id;
        chairButton.clicked += () => gridData.startBuildMode(chairId);

        Button tableButton = root.Q<Button>("tableButton");
        string tableId = objectDatabaseController.GetObjectDataByName("Table").Id;
        tableButton.clicked += () => gridData.startBuildMode(tableId);

        Button beltButton = root.Q<Button>("beltButton");
        string beltId = objectDatabaseController.GetObjectDataByName("Conveyor").Id;
        beltButton.clicked += () => gridData.startBuildMode(beltId);

        Button deleteButton = root.Q<Button>("deleteButton");
        deleteButton.clicked += () => gridData.startDeleteMode();

        Button startButton = root.Q<Button>("startButton");
        startButton.clicked += () => {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyMovement>().canStart = true;

            }
            gridData.endBuildMode();
        };
    }
}
