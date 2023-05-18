using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public Transform target; // The target character controller
    public float detectionRange = 10f; // Range within which the target is detected
    public float stoppingDistance = 2f; // Distance at which the enemy stops moving
    public float speed = 3f; // Movement speed

    private Rigidbody rb;
    private float groundY; // Fixed y position
    private bool hasHitTarget = false; // Variable to track if the enemy has touched the target

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundY = transform.position.y; // Store the initial y position as the ground level
    }

    private void Update()
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
        else
        {
            // Stop moving if target is out of range
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; // Stop rotation as well
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
