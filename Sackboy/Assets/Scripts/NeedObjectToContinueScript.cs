using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedObjectToContinueScript : MonoBehaviour
{
    public GameObject requiredObject;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public float pivotSpeed = 50.0f; // The speed at which the door pivots
    public float pivotAngle = 80.0f; // The angle to which the door pivots

    private bool isOpening = false;
    private bool isClosing = false;

    public bool ShowTutorial = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == requiredObject)
        {
            Debug.Log("Object placed on trigger pad!");
            isOpening = true;
            isClosing = false;
            ShowTutorial = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == requiredObject)
        {
            Debug.Log("Object removed from trigger pad!");
            isOpening = false;
            isClosing = true;
        }
    }

    private void Update()
    {
        if (isOpening)
        {
            ShowTutorial = false;
            float leftDoorTargetRotation = -pivotAngle;
            float rightDoorTargetRotation = pivotAngle;

            // Smoothly pivot the doors to the desired rotation
            float leftDoorRotation = Mathf.MoveTowardsAngle(leftDoor.transform.localEulerAngles.y,
                leftDoorTargetRotation, pivotSpeed * Time.deltaTime);
            float rightDoorRotation = Mathf.MoveTowardsAngle(rightDoor.transform.localEulerAngles.y,
                rightDoorTargetRotation, pivotSpeed * Time.deltaTime);

            // Set the new door rotations
            leftDoor.transform.localEulerAngles = new Vector3(leftDoor.transform.localEulerAngles.x, leftDoorRotation,
                leftDoor.transform.localEulerAngles.z);
            rightDoor.transform.localEulerAngles = new Vector3(rightDoor.transform.localEulerAngles.x,
                rightDoorRotation, rightDoor.transform.localEulerAngles.z);
        }
        else if (isClosing)
        {
            ShowTutorial = true;
            float leftDoorTargetRotation = 0.0f;
            float rightDoorTargetRotation = 0.0f;

            // Smoothly pivot the doors to the desired rotation
            float leftDoorRotation = Mathf.MoveTowardsAngle(leftDoor.transform.localEulerAngles.y,
                leftDoorTargetRotation, pivotSpeed * Time.deltaTime);
            float rightDoorRotation = Mathf.MoveTowardsAngle(rightDoor.transform.localEulerAngles.y,
                rightDoorTargetRotation, pivotSpeed * Time.deltaTime);

            // Set the new door rotations
            leftDoor.transform.localEulerAngles = new Vector3(leftDoor.transform.localEulerAngles.x, leftDoorRotation,
                leftDoor.transform.localEulerAngles.z);
            rightDoor.transform.localEulerAngles = new Vector3(rightDoor.transform.localEulerAngles.x,
                rightDoorRotation, rightDoor.transform.localEulerAngles.z);
        }
    }
}
