using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset = new Vector3(0f, 10f, -15f); // The offset from the target's position
    public float offsetSmoothness = 3f; // The smoothness factor for offset transition
    public Transform target2;
    void LateUpdate()
    {
        if (!target) return; // If there is no target, do nothing

        if (CharacterMovement.characterMovement.BigBossDead == true && CharacterMovement.characterMovement.takeOff == true)
        {
            target = target2;
        }

        // Adjust the offset based on the camera zoom state
        if (CharacterMovement.characterMovement.ZoomCamera == true)
        {
            offset.y = Mathf.Lerp(offset.y, 25.0f, offsetSmoothness * Time.deltaTime);
        }
        else
        {
            offset.y = Mathf.Lerp(offset.y, 10.0f, offsetSmoothness * Time.deltaTime);
        }

        if (CharacterMovement.characterMovement.SwitchCamera == true)
        {
            offset.z = Mathf.Lerp(offset.z, 15.0f, offsetSmoothness * Time.deltaTime);
            offset.y = Mathf.Lerp(offset.y, 10.0f, offsetSmoothness * Time.deltaTime);
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Set the camera's position to the desired position
        transform.position = desiredPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
