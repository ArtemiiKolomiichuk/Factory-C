using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot 
{
    [SerializeField]
    private Resource itemData;   

    [SerializeField]
    private int stackSize;

    public Resource ItemData => itemData;
    public int StackSize => stackSize;
    public InventorySlot(Resource itemData, int stackSize)
    {
        this.itemData = itemData;
        this.stackSize = stackSize;
    }

    public InventorySlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(Resource data, int amount)
    {
        itemData = data;
        stackSize = amount;
    }

    
}
