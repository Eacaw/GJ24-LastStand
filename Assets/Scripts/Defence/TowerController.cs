using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int health = 2; // Initial health of the tower
    public int damage = 1; // Damage amount to be dealt to the enemy
    public float range = 2f; // Range within which the tower deals damage to enemies
    public float damageInterval = 1f; // Time interval between damage applications
    public bool isObstacle = false;

    void Start()
    {
        damageCooldown = 0f;
    }

    void Update()
    {
        if (!isObstacle)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    rotation,
                    Time.deltaTime * 2
                );
            }

            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0f)
            {
                DealDamageToEnemiesInRange();
                damageCooldown = damageInterval;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private float damageCooldown;

    public void TakeDamage(int damage)
    {
        Debug.Log("Tower Damage");
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false); // Deactivate the tower instead of destroying it
        }
    }

    void DealDamageToEnemiesInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);
        Animator animator = gameObject.GetComponent<Animator>();

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(damage);
                }
            }
        }
    }
}
