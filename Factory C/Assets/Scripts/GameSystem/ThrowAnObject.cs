using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnObject : MonoBehaviour
{
    public GameObject[] objectPrefab;
    public GameObject player;
    public float throwForce = 3f;
    public Vector3 throwOffset = new Vector3(0, 1, 1);
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            InventoryHolder inventoryHolder = player.GetComponent<InventoryHolder>();
            if (inventoryHolder != null)
            {
                var item = inventoryHolder.InventorySystem.InventorySlots[0].ItemData;
                if (item != null)
                {
                    foreach (var a in objectPrefab)
                    {
                        if (a.GetComponent<ItemPickUp>().ItemData.Name == item.Name)
                        {
                            GameObject thrownObject = Instantiate(a, player.transform.position + player.transform.TransformDirection(throwOffset), player.transform.rotation);
                            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

                            if (rb != null)
                            {
                                rb.AddForce(player.transform.forward * throwForce, ForceMode.Impulse);
                            }
                           // inventoryHolder.InventorySystem.InventorySlots[0].ClearSlot();
                        }
                    }
                }
            }
        }
       


    }
    
}

