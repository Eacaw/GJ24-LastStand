using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public float detectionRadius = 1f;  // Radius to detect obstacles
    public float avoidanceStrength = 2f;  // Strength of avoidance maneuver
    public bool canStart = false;

    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        target = FindNearestTarget();

        if (target != null && canStart == true)
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
            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
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
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 avoidanceDirection = GetAvoidanceDirection();
        Vector3 moveDirection = (direction + avoidanceDirection).normalized;

        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
    }

    Vector3 GetAvoidanceDirection()
    {
        Vector3 avoidance = Vector3.zero;
        Collider[] obstacles = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider obstacle in obstacles)
        {
            if (obstacle.CompareTag("Obstacle"))
            {
                Vector3 directionToObstacle = transform.position - obstacle.transform.position;
                avoidance += directionToObstacle / directionToObstacle.sqrMagnitude;  // Weighted by distance
            }
        }

        // Check if avoidance is zero and try alternative directions
        if (avoidance == Vector3.zero)
        {
            Vector3[] alternativeDirections = {
                transform.right, -transform.right,
                transform.forward, -transform.forward,
                (transform.right + transform.forward).normalized,
                (-transform.right + transform.forward).normalized,
                (transform.right - transform.forward).normalized,
                (-transform.right - transform.forward).normalized
            };

            foreach (var altDir in alternativeDirections)
            {
                if (!Physics.Raycast(transform.position, altDir, detectionRadius, LayerMask.GetMask("Obstacle")))
                {
                    return altDir * avoidanceStrength;
                }
            }
        }

        return avoidance * avoidanceStrength;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }
}
