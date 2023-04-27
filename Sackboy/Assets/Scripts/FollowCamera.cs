using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The target to follow
    public float distance = 15.0f; // The distance from the target
    public float height = 10.0f; // The height of the camera above the target
    public float rotationDamping = 1.0f; // The speed at which the camera rotates
    public float heightDamping = 1.0f; // The speed at which the camera moves up and down
    void LateUpdate()
    {
        if (!target) return; // If there is no target, do nothing

        float wantedRotationAngle = target.eulerAngles.y; // Get the target's y-rotation
        float wantedHeight = target.position.y + height; // Calculate the height of the camera above the target

        float currentRotationAngle = transform.eulerAngles.y; // Get the camera's y-rotation
        float currentHeight = transform.position.y; // Get the camera's height

        // Smoothly rotate the camera towards the target's rotation
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Smoothly move the camera towards the target's height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Set the camera's position and rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        Vector3 newPosition = target.position;
        newPosition -= currentRotation * Vector3.forward * distance;
        newPosition.y = currentHeight;

        transform.position = newPosition;
        transform.LookAt(target);
    }
}
