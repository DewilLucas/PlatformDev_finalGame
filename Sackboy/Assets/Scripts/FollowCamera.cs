using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The target to follow
    public float distance = 20.0f; // The distance from the target
    public float height = 15.0f; // The height of the camera above the target
    public float rotationDamping = 5.0f; // The speed at which the camera rotates
    public float heightDamping = 5.0f; // The speed at which the camera moves up and down
    public float wallDetectionDistance = 2.0f; // The distance to detect walls
    public float topViewHeight = 30.0f; // The height of the camera in top view
    public float topViewAngle = 90.0f; // The angle of the camera in top view
    public float topViewDistance = 10.0f; // The distance of the camera in top view

    private bool isCloseToWall = false; // Whether the camera is close to a wall

    void LateUpdate()
    {
        if (!target) return; // If there is no target, do nothing

        float wantedRotationAngle;
        float wantedHeight;
        float currentRotationAngle;
        float currentHeight;

        // Check if the camera is close to a wall
        RaycastHit hit;
         if (Physics.Raycast(target.position, -transform.forward, out hit, wallDetectionDistance))
        {
            isCloseToWall = true;
        }
        else
        {
            isCloseToWall = false;
        }

        if (isCloseToWall)
        {
            // Switch to top view
            wantedRotationAngle = topViewAngle;
            wantedHeight = topViewHeight;
            distance = topViewDistance;
        }
        else
        {
            // Switch back to follow view
            wantedRotationAngle = target.eulerAngles.y;
            wantedHeight = target.position.y + height;
            distance = 20.0f;
        }

        currentRotationAngle = transform.eulerAngles.y; // Get the camera's y-rotation
        currentHeight = transform.position.y; // Get the camera's height

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
