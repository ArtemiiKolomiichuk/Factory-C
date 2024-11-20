using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAndStoreInteractible : MonoBehaviour, Interactible_v2
{
    public GameObject usableObject = null;
    public UsableInterface usableObjectInterface = null;
    public KeyCode useKey = KeyCode.None;

    public GameObject resourceHolderObject = null;
    public UniverslaResourceHolderInterface resourceHolder = null;
    public KeyCode getKey = KeyCode.None;
    public KeyCode putKey = KeyCode.None;

    private void Start()
    {
        if (usableObject != null) { 
            usableObjectInterface = usableObject.GetComponent<UsableInterface>();
        }
        if (resourceHolderObject != null)
        {
            resourceHolder = resourceHolderObject.GetComponent<UniverslaResourceHolderInterface>();
        }
    }

    public object Interact(KeyCode key, object data)
    {
        if (key == KeyCode.None) 
        {
            return false;
        }
        else if (key == useKey)
        {
            Debug.Log("Using item");
            return usableObjectInterface.Use();
        }
        else if (key == getKey)
        {
            Debug.Log("Getting resources");
            if (resourceHolder.HaveResource()) 
            { 
                return resourceHolder.PullResourceType();
            }
            return ResourceType.None;
        }
        else if (key == putKey)
        {
            Debug.Log("Putting resources");
            if (resourceHolder.HaveFreeSpace()) { 
                resourceHolder.PutResourceType((ResourceType)data);
                return true;
            }
            return false; 
        }
        else
        {
            Debug.Log("Key not recognized for interaction");
            return false;
        }
    }
}
