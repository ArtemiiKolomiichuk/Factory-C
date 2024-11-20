using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct InteractionResult
{
    public bool Success { get; }
    public object Data { get; }

    public InteractionResult(bool success, object data)
    {
        Success = success;
        Data = data;
    }
}

public class UserInteraction : MonoBehaviour
{
    void Start()
    {
        
    }  

    public List<Interactible_v2> targets = new List<Interactible_v2>();

    public InteractionResult InteractWithHead(KeyCode key, object data) {
        if (targets.Count > 0) 
        {
            return new InteractionResult(true, targets[0].Interact(key, data));
        }
        return new InteractionResult(false, null);
    }
    
    public void AddToTargets(Interactible_v2 inter) 
    {
        bool wasZero = targets.Count == 0;

        targets.Add(inter);
        if (wasZero)
        {
            targets[0].BehaveOnTargeted(this);
        }
    }

    public void RemoveFromTargets(Interactible_v2 inter) 
    {
        if (targets.Count <= 0) {
            return;
        }

        bool removedFirst = inter == targets[0]; 
        
        inter.BehaveOnUnTargeted(this);
        targets.Remove(inter);
        if (removedFirst && targets.Count > 0) 
        {
            targets[0].BehaveOnTargeted(this);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED TRIGGER: "+ other.name);
        Interactible_v2 interactable = other.GetComponent<Interactible_v2>();
        if (interactable != null)
        {
            AddToTargets(interactable);
            Debug.Log($"Interactable object entered trigger: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        Interactible_v2 interactable = other.GetComponent<Interactible_v2>();
        if (interactable != null)
        {
            RemoveFromTargets(interactable);
            Debug.Log($"Interactable object exited trigger: {other.gameObject.name}");
        }
    }

}
