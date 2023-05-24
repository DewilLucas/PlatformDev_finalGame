using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossEnnemyAIController : MonoBehaviour
{
    public Transform target; // The target character controller
    public float detectionRange = 30f; // Range within which the target is detected
    public float stoppingDistance = 5f; // Distance at which the enemy stops moving
    public float speed = 6f; // Movement speed

    private Rigidbody rb;
    private float groundY; // Fixed y position
    public bool isTargetInLastFloor = false; // Variable to track if the enemy has touched the target
    public bool hasHitTarget = false; // Variable to track if the enemy has touched the target

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundY = transform.position.y; // Store the initial y position as the ground level
    }

    private void Update()
    {
        if (isTargetInLastFloor== true)
        {
            if (IsTargetInRange())
            {
                Vector3 targetPosition = target.position;
                targetPosition.y = groundY; // Set the target position with fixed y

                Vector3 direction = targetPosition - transform.position;
                direction.Normalize();

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget > stoppingDistance)
                {
                    Vector3 movement = direction * speed;
                    rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

                    // Rotate to face the target
                    if (direction != Vector3.zero)
                    {
                        Quaternion toRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10f * Time.deltaTime);
                    }
                    hasHitTarget = false;
                }
                else
                {
                    if (target.position.y >5f)
                    {
                        Debug.Log("Target is above");
                        hasHitTarget = false;
                    }
                    else
                    {
                        // Target reached, stop moving
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero; // Stop rotation as well

                        // Set the hit variable to true
                        hasHitTarget = true;
                    }
                    
                }
            }
            else
            {
                // Stop moving if target is out of range
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero; // Stop rotation as well
            }
        }
        
    }

    private bool IsTargetInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= detectionRange;
        }
        return false;
    }

    public bool HasHitTarget()
    {
        return hasHitTarget;
    }
    
}
