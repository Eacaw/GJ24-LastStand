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

    private Vector3[] spawnPoints;

    private Waves waves;
    public int currentWave = 0;
    private bool isSpawning = false;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public UIDocument UIDocument;
    private VisualElement root;
    private Label waveCounter;
    private PlayerController playerController;

    void Start()
    {
        root = UIDocument.rootVisualElement;
        waveCounter = root.Q<Label>("WaveCount");
        spawnPoints = new Vector3[8];
        spawnPoints[0] = new Vector3(-15, 0, 0);
        spawnPoints[1] = new Vector3(15, 0, 0);
        spawnPoints[2] = new Vector3(0, 0, 15);
        spawnPoints[3] = new Vector3(0, 0, -15);
        spawnPoints[4] = new Vector3(12.5f, 0, 12.5f);
        spawnPoints[5] = new Vector3(-12.5f, 0, 12.5f);
        spawnPoints[6] = new Vector3(12.5f, 0, -12.5f);
        spawnPoints[7] = new Vector3(-12.5f, 0, -12.5f);

        waves = GetComponent<Waves>();
        if (waves == null)
        {
            Debug.LogError("Waves component not found on this GameObject.");
        }
        GameObject player = GameObject.Find("PlayerController");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
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
            yield return StartCoroutine(SpawnWave(currentWave));
            yield return new WaitUntil(() => activeEnemies.Count == 0);
            currentWave++;
            waveCounter.text = (currentWave + 1).ToString();
            playerController.AddScore(10);
            yield return new WaitForSeconds(waves.GetDelayBetweenWaves());
        }

        isSpawning = false;
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        int enemiesInWave = waves.GetEnemiesInWave(waveNumber);
        int[] enemyTypes = waves.GetWave(waveNumber).Enemies;

        for (int i = 0; i < enemiesInWave; i++)
        {
            int enemyType = enemyTypes[i];
            GameObject enemy = Instantiate(
                enemyPrefabs[enemyType],
                spawnPoints[Random.Range(0, 8)],
                Quaternion.identity
            );
            EnemyMovement enemyMovementScript = enemy.GetComponent<EnemyMovement>();
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyMovementScript != null && enemyScript != null)
            {
                ScaleEnemyStats(enemyMovementScript, waveNumber);
                enemyScript.OnEnemyDeath += HandleEnemyDeath;
            }
            else
            {
                Debug.LogError("Enemy prefab missing Enemy script.");
            }
            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void ScaleEnemyStats(EnemyMovement enemy, int waveNumber)
    {
        float scaleFactor = 1 + (waveNumber * 0.1f);
        enemy.health = (int)(enemy.health * scaleFactor);
        enemy.damage = (int)(enemy.damage * scaleFactor);
    }

    void SpawnEnemy(int enemyType)
    {
        GameObject enemy = Instantiate(
            enemyPrefabs[enemyType],
            spawnPoints[Random.Range(0, 8)],
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
