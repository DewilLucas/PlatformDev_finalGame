using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCharacterScript : MonoBehaviour
{
    public float jumpSpeed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public Animator anim;
    public static int DubbleJump = 0;
    private int _MaxDubbleJump = 2;
    public AudioSource audioSource;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            anim.SetBool("IsJumping", false);
            DubbleJump = 0;
            moveDirection.y = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
                DubbleJump++;
                if (DubbleJump <= _MaxDubbleJump)
                {
                    if (DubbleJump == _MaxDubbleJump)
                    {
                        audioSource.Play();
                        anim.SetBool("IsJumping", false);
                    }
                    else
                    {
                        audioSource.Play();
                        anim.SetBool("IsJumping", true);
                    }
                    moveDirection.y = jumpSpeed;
                }
                else
                {
                    DubbleJump = 3;
                }
        }
        //Debug.Log(DubbleJump);
        moveDirection.y -= 9.81f * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
