using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;

    public AudioSource Running;
   
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Animator anim;
    
    public  bool ZoomCamera = false;
    public  bool SwitchCamera = false;

    // Define a jump velocity that will be applied when the player lands on a jump pad
    public float jumpVelocity = 10.0f;

    // Keep track of whether the player is currently jumping
    private bool isJumping = false;

    // Define the speed at which the character rotates
    public float rotationSpeed = 10.0f;

    private bool IsJumpPadEnabled = true;



    public AiController[] Ennemies;
    public AiController[] Ennemies3rdFloor;
    private int _EnnemyHitCounter = 0;

    private float JumpPadTimer = 0.0f;



    private int _lives =5;
    private bool _isDead = false;
    private int deadAnimBlock = 0;
    public GameObject[] Lives;
    public MainGameScript mainGameScript;



    public GameObject TutorialWindow;
    public TMP_Text TutorialText;
    bool isWindowActive = false;
    float windowShowDuration = 2.0f; // Time in seconds to show the window
    float windowDownDuration = 0.5f; // Time in seconds to move the window down
    public NeedObjectToContinueScript needObjectToContinueScript;


    public GameObject DeadPopUp;

    public GameObject coinPrefab;

    private bool Spawn2 = false;
    
    public GameObject[] Ropes;
    public GameObject[] RopeCoins;
    public GameObject[] RopeCoins2;
    public AudioSource RopeSound;

    private bool AllEnemiesDead4thFloor = false;
    public GameObject[] Doors4thFloor;


    public BigBossEnnemyAIController BigBoss;
    private int BigBossLife = 10;
    public TMP_Text BigBossLifeText;
    public bool BigBossDead = false;
    public bool takeOff = false;

    private bool canPressV = true;
    public GameObject cubeToRemove;


    public TMP_Text txtEndGameFRocket;


    public GameObject WonPopUp;

    public AudioSource punchSound;

    public AudioSource jumpPadSound;
    public AudioSource biteSound;
    public AudioSource Doorsound;
    public AudioSource victorySound;

    public AudioSource RocketSound;
    public static CharacterMovement characterMovement;
    public AudioSource deadSound;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        txtEndGameFRocket.text = "";
        characterMovement = this;

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
                        if (Input.GetKeyDown(KeyCode.V) && canPressV)
                        {
                            punchSound.Play();
                            // Disable the ability to press the "V" key temporarily
                            canPressV = false;

                            // Delay coroutine for 2 seconds
                            StartCoroutine(DelayVPress());

                            ennemy.hasHitTarget = false;
                            float spawnOffset = 1.0f;
                            _EnnemyHitCounter = 0;
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
                                biteSound.Play();
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
                        }

                    }
                }
            }

            Debug.Log(BigBossLife);
            //BigBoss
            if (BigBoss != null)
            {
                BigBossLifeText.text = BigBossLife.ToString();
                if (BigBoss.hasHitTarget)
                {
                    if (Input.GetKeyDown(KeyCode.V) && canPressV)
                    {
                        punchSound.Play();
                        // Disable the ability to press the "V" key temporarily
                        canPressV = false;

                        // Delay coroutine for 2 seconds
                        StartCoroutine(DelayVPress());
                        BigBossLife--;
                        BigBoss.hasHitTarget = false;
                        _EnnemyHitCounter = 0;
                        // Spawn a coin at the adjusted position
                        if (BigBossLife <= 0)
                        {
                            BigBossLifeText.gameObject.SetActive(false);
                            BigBoss.gameObject.SetActive(false); // kill the ennemy
                            BigBossDead = true;

                            victorySound.Play();
                            if (!isWindowActive)
                            {
                                Debug.Log("Congratulations! You won! To leave the game enter the rocket!");
                                TutorialWindow.SetActive(true);
                                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                                isWindowActive = true;
                                windowShowDuration = 2.0f;
                                // Change the text of the window
                                TutorialText.text = "Congratulations! You won! To leave the game enter the rocket!";
                                txtEndGameFRocket.text = "F";
                            }
                            else
                            {
                                // Reset the timer and show the window again
                                isWindowActive = false;
                                TutorialWindow.SetActive(false);
                                Debug.Log("Congratulations! You won! To leave the game enter the rocket!");
                                TutorialText.text = "Congratulations! You won! To leave the game enter the rocket!";
                                TutorialWindow.SetActive(true);
                                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                                isWindowActive = true;
                                windowShowDuration = 2.0f;
                                txtEndGameFRocket.text = "F";
                                // Change the text of the window
                            }
                        }
                    }
                    else
                    {
                        _EnnemyHitCounter++;
                        if (_EnnemyHitCounter == 50)
                        {
                            biteSound.Play();
                            _EnnemyHitCounter = 0;
                            _lives--;
                            Lives[_lives].SetActive(false);
                            if (_lives <= 0)
                            {
                                _lives = 0;
                                _isDead = true;
                                deadSound.Play();
                                anim.SetBool("IsDead", true);
                                deadAnimBlock++;
                            }
                        }
                        Debug.Log(_lives);
                    }

                }
            }








            JumpPadTimer += Time.deltaTime;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (SwitchCamera)
            {
                horizontal *= -1f;
                vertical *= -1f;
            }
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
                if (!Running.isPlaying)
                {
                    Running.Play();
                }
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
                Running.Stop();
                anim.SetBool("IsMoving", false);
            }

            // Check for jump pads
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);
            foreach (Collider collider in colliders)
            {
                
                if (collider.gameObject.CompareTag("JumpPad") && controller.isGrounded && IsJumpPadEnabled == true)
                {
                    jumpPadSound.Play();
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
                    SwitchCamera = true;
                }

                if (collider.gameObject.CompareTag("RopeTrigger1"))
                {
                    RopeSound.Play();
                    collider.gameObject.SetActive(false);
                    StartCoroutine(GrowRopeSmoothly(Ropes[0].transform));
                    foreach (var coin in RopeCoins)
                    {
                        coin.SetActive(true);
                    }
                }
                if (collider.gameObject.CompareTag("RopeTrigger2"))
                {
                    RopeSound.Play();
                    collider.gameObject.SetActive(false);
                    StartCoroutine(GrowRopeSmoothly(Ropes[1].transform));
                    foreach (var coin in RopeCoins2)
                    {
                        coin.SetActive(true);
                    }
                }

                if (collider.gameObject.CompareTag("DeActivate"))
                {
                    cubeToRemove.SetActive(false);
                }
                if (collider.gameObject.CompareTag("LastFloor"))
                {
                    Debug.Log("LastFloor");
                    if (!isWindowActive)
                    {
                        
                        Debug.Log("Kill the boss and leave the game in the rocket");
                        TutorialWindow.SetActive(true);
                        StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                        isWindowActive = true;
                        windowShowDuration = 2.0f;
                        // Change the text of the window
                        TutorialText.text = "Kill the boss and leave the game in the rocket!";
                        collider.gameObject.SetActive(false);
                    }
                    else
                    {
                        // Reset the timer and show the window again
                        isWindowActive = false;
                        TutorialWindow.SetActive(false);
                        Debug.Log("Kill the boss and leave the game in the rocket");
                        TutorialText.text = "Kill the boss and leave the game in the rocket!";
                        TutorialWindow.SetActive(true);
                        StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                        isWindowActive = true;
                        windowShowDuration = 2.0f;
                        collider.gameObject.SetActive(false);
                        // Change the text of the window
                    }
                    BigBoss.isTargetInLastFloor = true;
                }

                if (collider.gameObject.CompareTag("Rocket")&& BigBossDead == true && Input.GetKeyDown(KeyCode.F))
                {
                    RocketSound.Play();
                    takeOff = true;
                    txtEndGameFRocket.text = "";
                    WonPopUp.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
            // If the jump pad is disabled, wait 2 seconds before enabling it again, this is to prevent the player from jumping multiple times in a row
            if (IsJumpPadEnabled == false && Math.Floor(JumpPadTimer) == 2)
            {
                JumpPadTimer = 0.0f;
                IsJumpPadEnabled = true;
            }
            bool[] ennemies3rdDead = new bool[Ennemies3rdFloor.Length];
            bool AllEnemiesDead4thFloor = true; // Assume all enemies are dead initially

            foreach (var FourthEnnemy in Ennemies3rdFloor)
            {
                if (!FourthEnnemy.gameObject.activeSelf)
                {
                    ennemies3rdDead[Array.IndexOf(Ennemies3rdFloor, FourthEnnemy)] = true;
                }
                else
                {
                    AllEnemiesDead4thFloor = false; // If any enemy is active, not all enemies are dead
                }
            }

            if (AllEnemiesDead4thFloor)
            {
                Doorsound.Play();
                Debug.Log("All enemies are dead, ready for boss");
                float pivotSpeed = 50.0f; // The speed at which the door pivots
                float pivotAngle = 80.0f; // The angle to which the door pivots
                                          //Doors4thFloor
                float leftDoorTargetRotation = -pivotAngle;
                float rightDoorTargetRotation = pivotAngle;

                // Smoothly pivot the doors to the desired rotation
                float leftDoorRotation = Mathf.MoveTowardsAngle(Doors4thFloor[0].transform.localEulerAngles.y,
                                              leftDoorTargetRotation, pivotSpeed * Time.deltaTime);
                float rightDoorRotation = Mathf.MoveTowardsAngle(Doors4thFloor[1].transform.localEulerAngles.y,
                                              rightDoorTargetRotation, pivotSpeed * Time.deltaTime);

                                          // Set the new door rotations
                Doors4thFloor[0].transform.localEulerAngles = new Vector3(Doors4thFloor[0].transform.localEulerAngles.x, leftDoorRotation, 
                    Doors4thFloor[0].transform.localEulerAngles.z);
                Doors4thFloor[1].transform.localEulerAngles = new Vector3(Doors4thFloor[1].transform.localEulerAngles.x,
                                              rightDoorRotation, Doors4thFloor[1].transform.localEulerAngles.z);
            }
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        

       
    }
   



    // Coroutine for delaying the "V" key press
    private IEnumerator DelayVPress()
    {
        yield return new WaitForSeconds(1.0f);
        canPressV = true;
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
            if (!isWindowActive)
            {
                Debug.Log("Kill every opponent you will encounter using V");
                TutorialWindow.SetActive(true);
                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                isWindowActive = true;
                windowShowDuration = 2.0f;
                // Change the text of the window
                TutorialText.text = "Kill every opponent you will encounter using V!";
            }
            else
            {
                // Reset the timer and show the window again
                isWindowActive = false;
                TutorialWindow.SetActive(false);
                Debug.Log("Kill every opponent you will encounter using V");
                TutorialText.text = "Kill every opponent you will encounter using V!";
                TutorialWindow.SetActive(true);
                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                isWindowActive = true;
                windowShowDuration = 2.0f;
                // Change the text of the window
            }
            ZoomCamera = true;
            SwitchCamera = false;
        }

        if (other.gameObject.CompareTag("3RDFloor"))
        {
            SwitchCamera = true;
            if (!isWindowActive)
            {
                Debug.Log("Kill every opponent you will encounter using V");
                TutorialWindow.SetActive(true);
                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                isWindowActive = true;
                windowShowDuration = 2.0f;
                // Change the text of the window
                TutorialText.text = "Kill every opponent to access the next level!";
            }
            else
            {
                // Reset the timer and show the window again
                isWindowActive = false;
                TutorialWindow.SetActive(false);
                Debug.Log("Kill every opponent you will encounter using V");
                TutorialText.text = "Kill every opponent to access the next level!";
                TutorialWindow.SetActive(true);
                StartCoroutine(PopWindow(TutorialWindow.transform, 0.5f, new Vector2(TutorialWindow.transform.position.x, 100)));
                isWindowActive = true;
                windowShowDuration = 2.0f;
                // Change the text of the window
            }
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
    private IEnumerator GrowRopeSmoothly(Transform ropeTransform)
    {
        float startY = ropeTransform.localScale.y;
        float targetY = 18f;
        float startX = ropeTransform.position.x;
        float targetX = 65f;
        float growthSpeed = 5f;

        while (ropeTransform.localScale.y < targetY)
        {
            float newY = Mathf.MoveTowards(ropeTransform.localScale.y, targetY, growthSpeed * Time.deltaTime);
            ropeTransform.localScale = new Vector3(ropeTransform.localScale.x, newY, ropeTransform.localScale.z);

            float newX = Mathf.MoveTowards(ropeTransform.position.x, targetX, growthSpeed * Time.deltaTime);
            ropeTransform.position = new Vector3(newX, ropeTransform.position.y, ropeTransform.position.z);

            yield return null;
        }
    }
}
