using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField]
    private int inventorySize;
    [SerializeField]
    protected InventorySystem inventorySystem;
    public Vector3 offset = new Vector3(0,1, 0);

    private GameObject current_show_item;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }

    private void Update()
    {
        if (current_show_item == null&&inventorySystem.GetInfo() != null)
        {
            GameObject item = PrefabSystem.FindItem(inventorySystem.GetInfo());
            //item.transform.SetParent(gameObject.transform);
            current_show_item = Instantiate(item, transform.position + offset, transform.rotation);
            current_show_item.transform.SetParent(transform);
            current_show_item.GetComponent<Rigidbody>().isKinematic = true;
            current_show_item.GetComponent<SphereCollider>().isTrigger = false;
            current_show_item.GetComponent<Rigidbody>().useGravity = false;
        }
        else if(current_show_item != null && inventorySystem.GetInfo() == null)
        {
            Destroy(current_show_item);
            current_show_item = null;
        }
    }

}
