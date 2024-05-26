using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canStart = false;
    public int damage = 1;
    public int health = 10;
    public float damageInterval = 1f;  // Time interval between damage applications

    // Algorithm Vars
    private List<Vector3> path;
    private int targetIndex;
    private Grid grid;

    private GameObject target;
    private Rigidbody rb;
    private float damageCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindNearestTarget();
        GameObject gridData = GameObject.Find("GridData");
        grid = gridData.GetComponent<Grid>();

        speed = Random.Range(3f, 7f);
        damageCooldown = 0f;
    }

    void Update()
    {
        // Check if the current target is still active
        if (target == null || !target.activeInHierarchy)
        {
            target = FindNearestTarget();
            path = null;  // Reset the path so it can be recalculated
        }

        if (target != null && grid != null && canStart && path == null)
        {
            CalculatePath();
        }

        damageCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (target != null && canStart && path != null)
        {
            MoveTowardsTarget();

            // Deal damage if in range and cooldown has elapsed
            if (damageCooldown <= 0f && Vector3.Distance(transform.position, target.transform.position) <= 0.5f)
            {
                DamageTarget();
                damageCooldown = damageInterval;  // Reset cooldown
            }
        }
    }

    GameObject FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        GameObject nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject potentialTarget in targets)
        {
            if (potentialTarget.activeInHierarchy)  // Only consider active targets
            {
                float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTarget = potentialTarget;
                }
            }
        }

        return nearestTarget;
    }

    void CalculatePath()
    {
        Vector3Int gridPos = GridPositionUtil.getGridFromWorld(transform.position, grid);
        Vector3Int targetGridPos = GridPositionUtil.getGridFromWorld(target.transform.position, grid);
        List<Vector3Int> intPath = Pathfinding.Instance.FindPath(gridPos, targetGridPos);
        path = new List<Vector3>();
        foreach (Vector3Int pos in intPath)
        {
            path.Add((Vector3)pos);
        }
        targetIndex = 0;
    }

    void MoveTowardsTarget()
    {
        if (path != null && targetIndex < path.Count)
        {
            Vector3 targetPosition = path[targetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        }

        // Damage target and deactivate if at the final position
        if (path != null && targetIndex >= path.Count && target != null)
        {
            if (damageCooldown <= 0f)
            {
                DamageTarget();
                damageCooldown = damageInterval;  // Reset cooldown
            }
        }
    }

    void DamageTarget()
    {
        TowerController tc = target.GetComponent<TowerController>();
        if (tc != null)
        {
            tc.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy -1");
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
