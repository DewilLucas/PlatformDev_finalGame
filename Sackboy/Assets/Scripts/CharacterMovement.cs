using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Animator anim;


    // Define a jump velocity that will be applied when the player lands on a jump pad
    public float jumpVelocity = 15.0f;

    // Keep track of whether the player is currently jumping
    private bool isJumping = false;

    // Define the speed at which the character rotates
    public float rotationSpeed = 10.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        controller.Move(Vector3.down * Time.deltaTime * 9.81f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            //controller.Move(moveDirection * speed * Time.deltaTime);
            if (GrabScript.grabScript.isGrabbing == true)
            {
                anim.SetBool("IsGrabbing", true);
                anim.SetBool("IsMoving", false);
            }
            else
            {
                anim.SetBool("IsGrabbing", false);
                anim.SetBool("IsMoving", true);
            }

        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        // Check for jump pads
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("JumpPad") && controller.isGrounded)
            {
                moveDirection.y = jumpVelocity;
                // Apply the jump velocity
                isJumping = true;
            }
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

    }
}
