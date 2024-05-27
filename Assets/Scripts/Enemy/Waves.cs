using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    private Wave[] waves;
    public int waveCount = 50;

    void Awake()
    {
        waves = GenerateWaves(waveCount);
    }

    public Wave GetWave(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waves.Length)
        {
            return waves[waveIndex];
        }
        else
        {
            // Handle invalid wave index
            return null;
        }
    }

    public int GetTotalWaves()
    {
        return waves.Length;
    }

    public float GetDelayBetweenWaves()
    {
        return 5f;
    }

    public float GetDelayBetweenEnemies()
    {
        return 1f;
    }

    public int GetEnemiesInWave(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waves.Length)
        {
            return waves[waveIndex].Enemies.Length;
        }
        else
        {
            // Handle invalid wave index
            return 0;
        }
    }

    public int GetEnemyType(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waves.Length)
        {
            return waves[waveIndex].Enemies[0];
        }
        else
        {
            // Handle invalid wave index
            return 0;
        }
    }

    private Wave[] GenerateWaves(int numberOfWaves)
    {
        List<Wave> generatedWaves = new List<Wave>();
        for (int i = 0; i < numberOfWaves; i++)
        {
            generatedWaves.Add(GenerateWave(i));
        }
        return generatedWaves.ToArray();
    }

    private Wave GenerateWave(int waveIndex)
    {
        List<int> enemies = new List<int>();

        // Adding lower-tier enemies to the start of each wave
        int lowerTierCount = Mathf.Min(5, waveIndex + 1);
        for (int i = 0; i < lowerTierCount; i++)
        {
            enemies.Add(0);
        }

        // Calculate the rest of the enemies
        int remainingEnemies = Mathf.Min(40, 5 + waveIndex);
        for (int i = lowerTierCount; i < remainingEnemies; i++)
        {
            int enemyType = CalculateEnemyType(waveIndex, i);
            enemies.Add(enemyType);
        }

        return new Wave(enemies.ToArray());
    }

    private int CalculateEnemyType(int waveIndex, int enemyIndex)
    {
        // Calculate enemy type based on wave index and enemy index
        if (waveIndex < 5)
        {
            return 0;
        }
        else if (waveIndex < 10)
        {
            return (enemyIndex % 2 == 0) ? 0 : 1;
        }
        else if (waveIndex < 15)
        {
            return (enemyIndex % 3 == 0)
                ? 0
                : (enemyIndex % 3 == 1)
                    ? 1
                    : 2;
        }
        else if (waveIndex < 20)
        {
            return (enemyIndex % 4 == 0)
                ? 0
                : (enemyIndex % 4 == 1)
                    ? 1
                    : (enemyIndex % 4 == 2)
                        ? 2
                        : 3;
        }
        else
        {
            return Random.Range(0, 4); // Randomize enemy types for higher waves
        }
    }
}

public class Wave
{
    public int[] Enemies { get; private set; }

    public Wave(int[] enemies)
    {
        Enemies = enemies;
    }
}
