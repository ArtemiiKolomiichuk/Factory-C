using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    List<Interactible_v2> targets = new List<Interactible_v2>();

    private void removeFromTarget() { 
    
    }

    private void changeCurrentTarget(Interactible_v2 newTarget) {
        //if (currentTarget != null) {
        //    currentTarget.BehaveOnUnTargeted();
        //}

        //currentTarget = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            Debug.Log($"Interactable object entered trigger: {other.gameObject.name}");
            // Handle the interactable object here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object has the Interactable component
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            Debug.Log($"Interactable object exited trigger: {other.gameObject.name}");
            // Handle the interactable object here
        }
    }

}
