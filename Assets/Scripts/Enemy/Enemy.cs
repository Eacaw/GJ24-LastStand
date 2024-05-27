using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    void OnDestroy()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);
        }
    }
}
