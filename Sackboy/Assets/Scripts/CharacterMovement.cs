using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Animator anim;
    
    public static bool ZoomCamera = false;

    // Define a jump velocity that will be applied when the player lands on a jump pad
    public float jumpVelocity = 10.0f;

    // Keep track of whether the player is currently jumping
    private bool isJumping = false;

    // Define the speed at which the character rotates
    public float rotationSpeed = 10.0f;

    private bool IsJumpPadEnabled = true;



    public AiController[] Ennemies;
    private int _EnnemyHitCounter = 0;

    private float JumpPadTimer = 0.0f;



    private int _lives =5;
    private bool _isDead = false;
    private int deadAnimBlock = 0;
    public GameObject[] Lives;
    public MainGameScript mainGameScript;



    public GameObject TutorialWindow;
    public TMP_Text TutorialText;
    private Coroutine moveWindowCoroutine;
    bool isWindowActive = false;
    float windowShowDuration = 2.0f; // Time in seconds to show the window
    float windowDownDuration = 0.5f; // Time in seconds to move the window down
    public NeedObjectToContinueScript needObjectToContinueScript;


    public GameObject DeadPopUp;

    public GameObject coinPrefab;

    private bool Spawn2 = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (isWindowActive)
        {
            windowShowDuration -= Time.deltaTime;

            if (windowShowDuration <= 0f)
            {
                StartCoroutine(PopWindow(TutorialWindow.transform, windowDownDuration, new Vector2(TutorialWindow.transform.position.x, -100)));
                isWindowActive = false;
            }
        }

        if (!mainGameScript.isPaused)
        {
            if (_isDead && deadAnimBlock == 1)
            {
                DeadPopUp.SetActive(true);
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("IsDead"))
                {
                    anim.Play("IsDead");
                    
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    anim.speed = 0f; // Pause the animation at the end
                }
                return;
            }
            if (Ennemies != null)
            {
                foreach (var ennemy in Ennemies)
                {
                    if (ennemy.hasHitTarget)
                    {
                        if (Input.GetKeyDown(KeyCode.V))
                        {
                            ennemy.hasHitTarget = false;
                            float spawnOffset = 1.0f;

                            // Calculate the spawn position with the offset
                            Vector3 spawnPosition = ennemy.transform.position + (ennemy.transform.forward * spawnOffset);

                            // Spawn a coin at the adjusted position
                            ennemy.gameObject.SetActive(false); // kill the ennemy

                            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                            coin.SetActive(true);
                        }
                        else
                        {
                            _EnnemyHitCounter++;
                            if (_EnnemyHitCounter == 50)
                            {
                                _EnnemyHitCounter = 0;
                                _lives--;
                                Lives[_lives].SetActive(false);
                                if (_lives <= 0)
                                {
                                    _lives = 0;
                                    _isDead = true;
                                    anim.SetBool("IsDead", true);
                                    deadAnimBlock++;
                                }
                            }
                            Debug.Log(_lives);
                        }

                    }
                }
            }


            JumpPadTimer += Time.deltaTime;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            // moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            // Only update the x and z components of moveDirection if the player is not jumping
            if (!isJumping)
            {
                moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            }
            else
            {
                // Keep the x and z components of moveDirection, and only update the y component
                //moveDirection += Physics.gravity * Time.deltaTime;

                controller.Move(Vector3.down * Time.deltaTime * 9.81f);
                if (controller.isGrounded || Math.Floor(moveDirection.y) <= 0)
                {
                    isJumping = false;
                }
            }
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

                if (collider.gameObject.CompareTag("JumpPad") && controller.isGrounded && IsJumpPadEnabled == true)
                {
                    JumpCharacterScript.DubbleJump = 2; // don't make the character jump again until they've landed
                    moveDirection.y = jumpVelocity;
                    // Apply the jump velocity
                    isJumping = true;
                    IsJumpPadEnabled = false;
                    JumpPadTimer = 0.0f;
                }

                if (collider.gameObject.CompareTag("DepositPad") && needObjectToContinueScript.ShowTutorial == true)
                {
                    if (!isWindowActive)
                    {
                        Debug.Log("You need to bring the cube!");
                        TutorialWindow.SetActive(true);
                        StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                        isWindowActive = true;
                        windowShowDuration = 2.0f;
                        // Change the text of the window
                        TutorialText.text = "To open the door you need to find, and deposit the cube here ! ";
                    }
                    else
                    {
                        // Reset the timer and show the window again
                        windowShowDuration = 2.0f;
                    }
                }

                if (collider.gameObject.CompareTag("Respawn"))
                {
                    Spawn2 = true;
                }


            }
            // If the jump pad is disabled, wait 5 seconds before enabling it again, this is to prevent the player from jumping multiple times in a row
            if (IsJumpPadEnabled == false && Math.Floor(JumpPadTimer) == 5)
            {
                JumpPadTimer = 0.0f;
                IsJumpPadEnabled = true;
            }
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        

       
    }

    void LateUpdate()
    {
        if (controller.transform.position.y <=-5 && Spawn2 == false)
        {
            controller.transform.position = new Vector3(0, 0, 0); // if the player falls off the map, reset their position to the origin
        }

        if (controller.transform.position.y <= -5 && Spawn2 == true)
        {
            controller.transform.position = new Vector3(15f, 20f, 80f); // if the player falls off the map, and its the second spawn, reset their position to the second spawn
            _lives--;
            Lives[_lives].SetActive(false);
            if (_lives <= 0)
            {
                _lives = 0;
                _isDead = true;
                anim.SetBool("IsDead", true);
                deadAnimBlock++;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ZoomCamera"))
        {
            ZoomCamera = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ZoomCamera"))
        {
            ZoomCamera = false;
        }
    }

    private IEnumerator PopWindow(Transform transformToPop, float duration, Vector2 targetPosition)
    {
        Vector2 initialPosition = transformToPop.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transformToPop.position = Vector2.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToPop.position = targetPosition;
    }

}
