using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCharacterScript : MonoBehaviour
{
    public float jumpSpeed = 18.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public Animator anim;



    private int _DubbleJump = 0;
    private int _MaxDubbleJump = 2;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            anim.SetBool("IsJumping", false);
            _DubbleJump = 0;
            moveDirection.y = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
                _DubbleJump++;
                if (_DubbleJump <= _MaxDubbleJump)
                {
                    moveDirection.y = jumpSpeed;
                    anim.SetBool("IsJumping", true);
                }
                else
                {
                    _DubbleJump = 3;
                }
        }
        
        Debug.Log(_DubbleJump);
        moveDirection.y -= 9.81f * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
