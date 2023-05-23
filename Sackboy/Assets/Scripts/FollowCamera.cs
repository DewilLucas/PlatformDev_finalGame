using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset = new Vector3(0f, 10f, -15f); // The offset from the target's position

    void LateUpdate()
    {
        if (!target) return; // If there is no target, do nothing

        
        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Set the camera's position to the desired position
        transform.position = desiredPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
