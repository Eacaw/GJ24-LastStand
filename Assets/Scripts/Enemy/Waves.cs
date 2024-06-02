using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    private Wave[] waves;
    public int waveCount = 50;
    public float waveDelay = 5f;

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
        return waveDelay;
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

        int enemyCount = CalculateEnemyCount(waveIndex);
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyType = CalculateEnemyType(waveIndex, i);
            enemies.Add(enemyType);
        }

        return new Wave(enemies.ToArray());
    }

    private int CalculateEnemyCount(int waveIndex)
    {
        // Gradually increase enemy count based on wave index
        // Using a logarithmic scale to increase the difficulty
        return Mathf.FloorToInt(5 + Mathf.Log(waveIndex + 1) * 5);
    }

    private int CalculateEnemyType(int waveIndex, int enemyIndex)
    {
        // Probability distribution for enemy types based on wave index
        float skeletonProbability = Mathf.Clamp(0.7f - waveIndex * 0.01f, 0.1f, 0.7f);
        float headlessProbability = Mathf.Clamp(0.2f + waveIndex * 0.005f, 0f, 0.3f);
        float blueSharkProbability = Mathf.Clamp(0.05f + waveIndex * 0.005f, 0f, 0.3f);
        float graySharkProbability = Mathf.Clamp(0.05f + waveIndex * 0.005f, 0f, 0.3f);

        float rand = Random.value;
        if (rand < skeletonProbability)
        {
            return 0;
        }
        else if (rand < skeletonProbability + headlessProbability)
        {
            return 1;
        }
        else if (rand < skeletonProbability + headlessProbability + blueSharkProbability)
        {
            return 2;
        }
        else
        {
            return 3;
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
