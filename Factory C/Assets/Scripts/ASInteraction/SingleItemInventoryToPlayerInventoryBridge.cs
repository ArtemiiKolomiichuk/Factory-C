using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleItemInventoryToPlayerInventoryBridge : MonoBehaviour, UniverslaResourceHolderInterface
{
    public GameObject inventoryHolderObject;
    InventoryHolder inventoryHolder = null;

    private void Start()
    {
        inventoryHolder = inventoryHolderObject.GetComponent<InventoryHolder>();
    }

    public void ClearResources()
    {
        if (!HaveFreeSpace()) 
        {
            PullResource();
        }
    }

    public int GetCountOfResources()
    {
        if (HaveResource()) return 1;
        return 0;
    }

    public bool HaveResource()
    {
        return !HaveFreeSpace();
    }

    public Resource PullResource()
    {
        Resource res = inventoryHolder.InventorySystem.GetInfo();
        inventoryHolder.InventorySystem.RemoveFromInventory();
        return res;
    }

    public bool HaveFreeSpace() { 
        return inventoryHolder.InventorySystem.HasFreeSlot(out InventorySlot freeSlot);
    }

    public int GetFreeSpaceCounter() {
        return 1 - GetCountOfResources();
    }

    public void PutResource(Resource resource)
    {
        inventoryHolder.InventorySystem.AddToInventory(resource);
    }

    public void PutResourceType(ResourceType resource)
    {
        if (resource != ResourceType.None) 
        {
            PutResource(ResourceController.Instance.resourceDictionary[resource]);
        }
    }

}
