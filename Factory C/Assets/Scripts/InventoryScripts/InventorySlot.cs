using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot 
{
    [SerializeField]
    private InventoryItemData itemData;   

    [SerializeField]
    private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;
    public InventorySlot(InventoryItemData itemData, int stackSize)
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

    
}
