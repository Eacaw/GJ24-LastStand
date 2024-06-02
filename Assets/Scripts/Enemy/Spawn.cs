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
    private ProgressBar progressBar;
    private VisualElement root;
    private Label waveCounter;
    private PlayerController playerController;

    void Start()
    {
        root = UIDocument.rootVisualElement;
        waveCounter = root.Q<Label>("WaveCount");
        progressBar = root.Q<ProgressBar>("NextWaveProgressBar");
        var actualBar = root.Q(className: "unity-progress-bar__progress");
        var bar__background = root.Q(className: "unity-progress-bar__background");
        actualBar.style.backgroundColor = new Color(0, 1, 0, 0.5f);
        bar__background.style.backgroundColor = new Color(2, 2, 2, 0.3f);

        progressBar.style.display = DisplayStyle.None; // Hide the progress bar initially

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

        while (currentWave < waves.GetTotalWaves())
        {
            // Show and fill the progress bar between waves
            yield return StartCoroutine(FillProgressBar(waves.GetDelayBetweenWaves()));

            yield return StartCoroutine(SpawnWave(currentWave));
            yield return new WaitUntil(() => activeEnemies.Count == 0);
            currentWave++;
            waveCounter.text = (currentWave + 1).ToString();
            playerController.AddScore(10);
        }

        isSpawning = false;
    }

    IEnumerator FillProgressBar(float delay)
    {
        progressBar.style.display = DisplayStyle.Flex;
        progressBar.value = 0;

        float elapsedTime = 0;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / delay) * 100; // Scale the value to be between 0 and 100
            yield return null;
        }

        progressBar.value = 100; // Ensure the progress bar is full at the end
        yield return new WaitForSeconds(0.5f); // Add a small delay to show the filled progress bar before hiding it

        progressBar.style.display = DisplayStyle.None;
        progressBar.value = 0;
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
