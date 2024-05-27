using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    public int maxEnemies = 10;
    public int enemiesSpawned = 0;

    public bool canStart = false;

    private float timer = 0f;

    private Vector3[] spawnPoints;

    void Start()
    {
        spawnPoints = new Vector3[maxEnemies];
        spawnPoints[0] = new Vector3(-15, 0, 0);
        spawnPoints[1] = new Vector3(15, 0, 0);
        spawnPoints[2] = new Vector3(0, 0, 15);
        spawnPoints[3] = new Vector3(0, 0, -15);
    }

    void Update()
    {
        if (canStart)
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && enemiesSpawned < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoints[enemiesSpawned % 4],
            Quaternion.identity
        );
        enemiesSpawned++;
    }
}
