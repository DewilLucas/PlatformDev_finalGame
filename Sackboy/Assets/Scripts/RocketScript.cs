using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public GameObject EndGameFRocket;
    // Update is called once per frame
    void Update()
    {
        if (CharacterMovement.characterMovement.takeOff == true)
        {
            StartCoroutine(MoveRocketInSky());
        }
    }
    private System.Collections.IEnumerator MoveRocketInSky()
    {
        // Define the duration and the target position for the rocket
        float duration = 2.0f; // Adjust the duration as needed
        Vector3 targetPosition = new Vector3(EndGameFRocket.transform.position.x, EndGameFRocket.transform.position.y + 100, EndGameFRocket.transform.position.z);

        // Store the initial position of the rocket
        Vector3 initialPosition = EndGameFRocket.transform.position;

        // Keep track of the elapsed time
        float elapsedTime = 0.0f;

        // Move the rocket smoothly over the defined duration
        while (elapsedTime < duration)
        {
            // Calculate the normalized progress (0 to 1) based on the elapsed time and duration
            float normalizedTime = elapsedTime / duration;

            // Interpolate the rocket's position using the initial and target positions
            EndGameFRocket.transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure that the rocket reaches the target position precisely
        EndGameFRocket.transform.position = targetPosition;
    }

}
