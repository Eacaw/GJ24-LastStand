using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canStart = false;

    // Algorithm Vars
    private List<Vector3> path;
    private int targetIndex;
    public Grid grid;

    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindNearestTarget();
        GameObject test = GameObject.Find("GridData");
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

        // If there is no path yet and we can start, then determine path
        if (target != null && grid != null && canStart == true && path == null)
        {
            Vector3Int gridPos = GridPositionUtil.getGridFromWorld(transform.position, this.grid);
            Vector3Int targetGridPos = GridPositionUtil.getGridFromWorld(target.position, this.grid);
            List<Vector3Int> intPath = Pathfinding.Instance.FindPath(gridPos, targetGridPos);
            path = new List<Vector3>();
            foreach (Vector3Int pos in intPath)
            {
                path.Add((Vector3)pos);
            }
            targetIndex = 0;
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
            Vector3 targetPosition = path[targetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }
}
