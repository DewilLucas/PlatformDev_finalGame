using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    public float grabRange = 3f;
    public float throwForce = 10f;
    public Transform holdPoint;

    private GameObject heldObject;
    public Animator animator;

    public bool isGrabbing = false;
    public static GrabScript grabScript;

    public GameObject PressF;
    public AudioSource GrabSound;
    public AudioSource ThrowSound;
    void Start()
    {
        grabScript = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObject == null)
            {
                GrabSound.Play();
                // Try to pick up object
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Grabbable"))
                    {
                        isGrabbing = true;
                        heldObject = hitCollider.gameObject;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject.transform.position = holdPoint.position;
                        heldObject.transform.parent = holdPoint;
                        PressF.SetActive(false);
                        break;
                    }
                }
            }
            else
            {
                ThrowSound.Play();
                isGrabbing = false;
                // Throw the held object
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
                heldObject = null;
            }
        }

        // Move the held object with the hold point
        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            isGrabbing= true;
        }
    }
}
