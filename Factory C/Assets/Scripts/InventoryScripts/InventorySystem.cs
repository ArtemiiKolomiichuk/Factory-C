using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


[System.Serializable]
public class InventorySystem
{
    [SerializeField]
    private List<InventorySlot> inventorySlots;

    private int inventory_size = 1;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;
    public UnityAction<Resource> OnInventoryUpdated;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for(int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public Resource GetInfo()
    {
        return InventorySlots[0].ItemData;
    }


    public bool AddToInventory(Resource itemToAdd)
    {
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, 1);
            OnInventorySlotChanged?.Invoke(freeSlot);
            OnInventoryUpdated?.Invoke(GetInfo());
            return true;
        }
        else return false;

    }

    public bool RemoveFromInventory()
    {
        Debug.Log("Removing item from inventory");
        InventorySlot occupiedSlot = InventorySlots.FirstOrDefault(i => i.ItemData != null);

        if (occupiedSlot != null)
        {
            occupiedSlot.ClearSlot();
            OnInventorySlotChanged?.Invoke(occupiedSlot);
            OnInventoryUpdated?.Invoke(GetInfo());
            return true;
        }
        else
        {
            Debug.Log("No item to remove");
            return false;
        }
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i =>i.ItemData==null);
        return freeSlot==null ? false : true;
    }
}
