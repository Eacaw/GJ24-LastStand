using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int health = 2; // Initial health of the target
    public int damage = 1;

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
