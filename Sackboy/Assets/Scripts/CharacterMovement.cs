using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 1.0f;
    private bool isGrounded = false;
    private Rigidbody rb;
    private int DoubleJump = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true )
        {
            DoubleJump++;
            if (DoubleJump <= 2)
            {
                Debug.Log(DoubleJump);
                isGrounded = true;
                if (DoubleJump == 1)
                {
                    rb.AddForce(Vector3.up * (jumpForce * 1f), ForceMode.Impulse);
                }

                if (DoubleJump > 1)
                {
                    rb.AddForce(Vector3.up * (jumpForce * 0.5f), ForceMode.Impulse);
                }
            }
            else
            {
                isGrounded = false;
                DoubleJump = 0;
            }
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3) // IF he's touching the ground
        {
            isGrounded = true;
        }
    }
}
