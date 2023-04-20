using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 1.0f;
    private bool isGrounded = false;
    private Rigidbody rb;

    private int doubleJumpCount = 0;
    public int maxDoubleJumps = 1;

    
    public Animator anim;

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
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (isGrounded)
            {
                anim.SetBool("IsJumping", true);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            else if (doubleJumpCount < maxDoubleJumps)
            {
                rb.AddForce(Vector3.up * (jumpForce / 2), ForceMode.Impulse);
                doubleJumpCount++;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3) // IF he's touching the ground
        {
            isGrounded = true;
            anim.SetBool("IsJumping", !isGrounded);
            doubleJumpCount = 0;
        }
    }
}
