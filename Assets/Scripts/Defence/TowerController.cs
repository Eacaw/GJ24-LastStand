using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int health = 2; // Initial health of the target
    public int damage = 1;
    public bool isObstacle = false;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Animator animator = collision.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            TakeDamage(); // Decrease health by 1 when collided with an enemy
        }
    }

    void TakeDamage()
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject); // Remove the object when health reaches 0
        }
    }
}
