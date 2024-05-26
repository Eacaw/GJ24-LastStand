using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int health = 2;  // Initial health of the tower
    public int damage = 2;  // Damage amount to be dealt to the enemy
    public float range = 2f;  // Range within which the tower deals damage to enemies
    public float damageInterval = 1f;  // Time interval between damage applications

    private float damageCooldown;

    void Start()
    {
        damageCooldown = 0f;
    }

    void Update()
    {
        damageCooldown -= Time.deltaTime;

        if (damageCooldown <= 0f)
        {
            DealDamageToEnemiesInRange();
            damageCooldown = damageInterval;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Tower Damage");
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);  // Deactivate the tower instead of destroying it
        }
    }

    void DealDamageToEnemiesInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(damage);
                }
            }
        }
    }
}
