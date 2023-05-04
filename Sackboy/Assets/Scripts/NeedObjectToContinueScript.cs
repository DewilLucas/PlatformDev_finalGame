using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedObjectToContinueScript : MonoBehaviour
{
    public GameObject requiredObject;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == requiredObject)
        {
            Debug.Log("Object placed on trigger pad!");
            // Perform any desired actions here...
        }
    }
}
