using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleItemInventory : MonoBehaviour, UniverslaResourceHolderInterface
{

    public void Update()
    {
        //DEBUG VVV
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PullResourceType();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            PutResourceType(ResourceType.Water);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            PutResourceType(ResourceType.Metal);
        }
    }

    public ItemDisplayer displayer = null;

    public ResourceType storedItem = ResourceType.None;

    public void ClearResources()
    {
        storedItem = ResourceType.None;
        if (displayer != null)
        {
            displayer.ShowResource(storedItem);
        }
    }

    public int GetCountOfResources()
    {
        if (HaveResource()) return 1;
        return 0;
    }

    public bool HaveResource()
    {
        return storedItem != ResourceType.None;
    }

    public ResourceType PullResourceType()
    {
        ResourceType toReturn = storedItem;
        storedItem = ResourceType.None;
        if (displayer != null)
        {
            displayer.ShowResource(storedItem);
        }
        return toReturn;
    }

    public bool HaveFreeSpace() { 
        return !HaveResource(); 
    }

    public int GetFreeSpaceCounter() {
        return 1 - GetCountOfResources();
    }

    public void PutResourceType(ResourceType resource)
    {
        storedItem = resource;
        if (displayer != null)
        {
            displayer.ShowResource(storedItem);
        }
    }

}
