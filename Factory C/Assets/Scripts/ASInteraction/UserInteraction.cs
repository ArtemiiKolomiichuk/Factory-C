using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteraction : MonoBehaviour
{
    void Start()
    {
        
    }

    public SingleItemInventory tmpPlayerInventory = null;

    void Update()
    {
        HandleInput();
    }

    //This should be in another class and will be VVV VVV VVV
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            InteractWithHead(KeyCode.Keypad1, true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            InteractWithHead(KeyCode.Keypad2, true);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            InteractWithHead(KeyCode.Keypad3, ResourceType.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            InteractWithHead(KeyCode.P, tmpPlayerInventory);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("USER_INTERACTION | "+ targets.Count);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (tmpPlayerInventory.HaveResource()) 
            {
                var getType = tmpPlayerInventory.PullResourceType();
                bool get = (bool) InteractWithHead(KeyCode.UpArrow, getType);
                if (!get) 
                { 
                    tmpPlayerInventory.PutResourceType(getType);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (tmpPlayerInventory.HaveFreeSpace())
            {
                ResourceType get = (ResourceType) InteractWithHead(KeyCode.DownArrow, true);
                tmpPlayerInventory.PutResourceType(get);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (tmpPlayerInventory.HaveFreeSpace())
            {
                InteractWithHead(KeyCode.RightArrow, true);
            }
        }
    }
    //This should be in another class and will be AAA AAA AAA

    public List<Interactible_v2> targets = new List<Interactible_v2>();

    public object InteractWithHead(KeyCode key, object data) {
        if (targets.Count > 0) 
        {
            return targets[0].Interact(key, data);
        }
        return false;
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
