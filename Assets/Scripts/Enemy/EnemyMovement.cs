using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canStart = true;

    // Algorithm Vars
    private List<Vector3> path;
    private int targetIndex;
    public Grid grid;

    private Transform target;
    private Rigidbody rb;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindNearestTarget();
        GameObject test = GameObject.Find("GridData");
        animator = GetComponent<Animator>();
        grid = test.GetComponent<Grid>();

        this.speed = Random.Range(3f, 7f);
    }

    void Update()
    {
        // If there is no target, get one!
        if (target == null)
        {
            target = FindNearestTarget();
        }

        // Rotate towards target
        if (target != null)
        {
            Vector3 targetDir = target.position - transform.position;
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        // If there is no path yet and we can start, then determine path
        if (target != null && grid != null && canStart == true && path == null)
        {
            Vector3Int gridPos = GridPositionUtil.getGridFromWorld(transform.position, this.grid);
            Vector3Int targetGridPos = GridPositionUtil.getGridFromWorld(
                target.position,
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
                MoveTowardsTarget(target.position);
            }
        }

        // If there is a path and a target get moving
        if (target != null && canStart == true && path != null)
        {
            MoveTowardsTarget();
        }
    }

    Transform FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject potentialTarget in targets)
        {
            float distance = Vector3.Distance(
                transform.position,
                potentialTarget.transform.position
            );
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = potentialTarget.transform;
            }
        }

        return nearestTarget;
    }

    void MoveTowardsTarget()
    {
        if (path != null && targetIndex < path.Count)
        {
            animator.SetBool("isWalking", true);
            Vector3 targetPosition = path[targetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
    }
}
