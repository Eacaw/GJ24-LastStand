using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int health = 2;  // Initial health of the target
    public int damage = 1;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);  // Remove the object when health reaches 0
        }
    }
}
