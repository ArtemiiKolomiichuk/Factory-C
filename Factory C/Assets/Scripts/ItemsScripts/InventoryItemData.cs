using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="InventorySystem/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int Id;
    public string Name;
    [TextArea(4,4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize = 1;
    public string[] ItemsToCreate;
}
