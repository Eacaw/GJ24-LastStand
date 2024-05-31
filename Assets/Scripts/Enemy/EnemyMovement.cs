using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canStart = true;
    public int damage = 1;
    public int health = 10;
    public float damageInterval = 1f; // Time interval between damage applications
    public int value = 1;
    public int scoreValue = 1;

    // Algorithm Vars
    private List<Vector3> path;
    private int targetIndex;
    private Grid grid;

    private GameObject target;
    private Rigidbody rb;
    private float damageCooldown;

    private Animator animator;
    public AudioClip damageSoundClip;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindNearestTarget();
        GameObject gridData = GameObject.Find("GridData");
        grid = gridData.GetComponent<Grid>();
        damageCooldown = 0f;
        animator = GetComponent<Animator>();

        GameObject player = GameObject.Find("PlayerController");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        // Check if the current target is still active
        if (target == null || !target.activeInHierarchy)
        {
            target = FindNearestTarget();
            path = null; // Reset the path so it can be recalculated
        }

        if (target != null && grid != null && canStart && path == null)
        {
            CalculatePath();
        }

        damageCooldown -= Time.deltaTime;

        // Rotate towards target
        if (target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    void FixedUpdate()
    {
        if (target != null && canStart && path != null)
        {
            MoveTowardsTarget();

            // Deal damage if in range and cooldown has elapsed
            if (
                damageCooldown <= 0f
                && Vector3.Distance(transform.position, target.transform.position) <= 0.5f
            )
            {
                DamageTarget();
                damageCooldown = damageInterval; // Reset cooldown
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
            if (potentialTarget.activeInHierarchy) // Only consider active targets
            {
                float distance = Vector3.Distance(
                    transform.position,
                    potentialTarget.transform.position
                );
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTarget = potentialTarget;
                }
            }
        }

        return nearestTarget;
    }

    public void checkForNewNearestTarget()
    {
        GameObject newTarget = FindNearestTarget();
        if (newTarget != null)
        {
            target = newTarget;
            path = null; // Reset the path so it can be recalculated
        }
    }

    void CalculatePath()
    {
        Vector3Int gridPos = GridPositionUtil.getGridFromWorld(transform.position, this.grid);
        Vector3Int targetGridPos = GridPositionUtil.getGridFromWorld(
            target.transform.position,
            this.grid
        );
        List<Vector3Int> intPath = Pathfinding.Instance.FindPath(gridPos, targetGridPos);
        path = new List<Vector3>();
        if (intPath != null)
        {
            foreach (Vector3Int pos in intPath)
            {
                path.Add((Vector3)pos);
            }
            targetIndex = 0;
        }
        else
        {
            path = null;
            target = FindNearestTarget();
            MoveTowardsTarget(target.transform.position);
        }
    }

    void MoveTowardsTarget()
    {
        if (path != null && targetIndex < path.Count)
        {
            animator.SetBool("isWalking", true);
            Vector3 targetPosition = path[targetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (Vector3.Distance(transform.position, path[path.Count - 1]) > 1)
            {
                rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        }

        // Damage target and deactivate if at the final position
        if (path != null && targetIndex >= (path.Count - 1) && target != null)
        {
            if (damageCooldown <= 0f)
            {
                DamageTarget();
                damageCooldown = damageInterval; // Reset cooldown
            }
        }
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        animator.SetBool("isWalking", true);
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetIndex++;
        }
    }

    void DamageTarget()
    {
        TowerController tc = target.GetComponent<TowerController>();
        if (tc != null)
        {
            tc.TakeDamage(damage);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            if (damageSoundClip != null)
            {
                AudioPoolManager.Instance.PlaySound(damageSoundClip, transform.position);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (playerController != null)
            {
                playerController.AddCurrency(value);
                playerController.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}
