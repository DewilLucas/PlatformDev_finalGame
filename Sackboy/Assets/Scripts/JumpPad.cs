using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public LayerMask characterLayer;
    public float jumpForce = 10f;

    void Start()
    {

    }
    /*void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.layer == 6)
        {
            CharacterController character = other.gameObject.GetComponent<CharacterController>();
            if (character != null)
            {
                character.Move(Vector3.up * jumpForce * Time.deltaTime);
            }
        }
    }*/
    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "BoxCollider")
        {
            // Handle collision with box collider here
            Debug.Log("Box collider hit!");
        }
    }
}
