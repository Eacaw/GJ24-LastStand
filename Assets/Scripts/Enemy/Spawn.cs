using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 1f;
    public int maxEnemies = 10;
    public int enemiesSpawned = 0;

    public bool canStart = false;

    private float timer = 0f;

    private Vector3[] spawnPoints;

    private Waves waves;
    public int currentWave = 0;
    private bool isSpawning = false;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public UIDocument UIDocument;
    private VisualElement root;
    private Label waveCounter;

    void Start()
    {
        root = UIDocument.rootVisualElement;
        waveCounter = root.Q<Label>("WaveCount");
        spawnPoints = new Vector3[maxEnemies];
        spawnPoints[0] = new Vector3(-15, 0, 0);
        spawnPoints[1] = new Vector3(15, 0, 0);
        spawnPoints[2] = new Vector3(0, 0, 15);
        spawnPoints[3] = new Vector3(0, 0, -15);

        waves = GetComponent<Waves>();
        if (waves == null)
        {
            Debug.LogError("Waves component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (canStart && !isSpawning)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        isSpawning = true;

        yield return new WaitForSeconds(waves.GetDelayBetweenWaves());

        while (currentWave < waves.GetTotalWaves())
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitUntil(() => activeEnemies.Count == 0);
            Debug.Log("Wave " + (currentWave) + " ended.");
            currentWave++;
            waveCounter.text = currentWave.ToString();
            yield return new WaitForSeconds(waves.GetDelayBetweenWaves());
        }

        isSpawning = false;
        Debug.Log("Game over you win!");
    }

    IEnumerator SpawnWave()
    {
        int enemiesInWave = waves.GetEnemiesInWave(currentWave);
        int[] enemyTypes = waves.GetWave(currentWave).Enemies;

        for (int i = 0; i < enemiesInWave; i++)
        {
            SpawnEnemy(enemyTypes[i]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(int enemyType)
    {
        GameObject enemy = Instantiate(
            enemyPrefabs[enemyType],
            spawnPoints[enemiesSpawned % 4],
            Quaternion.identity
        );
        enemiesSpawned++;
        activeEnemies.Add(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogError("Enemy prefab missing Enemy script.");
        }
    }

    void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        Destroy(enemy);
    }
}
