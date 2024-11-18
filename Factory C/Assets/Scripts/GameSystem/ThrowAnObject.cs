using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ThrowAnObject : MonoBehaviour
{
    
    public GameObject player;
    public float throwForce = 3f;
    public Vector3 throwOffset = new Vector3(0, 1, 1);

    private void Start()
    {
        if(NetworkCompanion.networkEnabled)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in players)
            {
                if (p.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    player = p;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InventoryHolder inventoryHolder = player.GetComponent<InventoryHolder>();
            if (inventoryHolder)
            {
                var item = inventoryHolder.InventorySystem.InventorySlots[0].ItemData;
                if (item != null)
                {
                    var a = PrefabSystem.FindItem(item);
                    if (a.GetComponent<ItemPickUp>().ItemData.rType == item.rType)
                    {
                        GameObject thrownObject = Instantiate(a, player.transform.position + player.transform.TransformDirection(throwOffset), player.transform.rotation);
                        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

                        if (!rb)
                        {
                            rb.AddForce(player.transform.forward * throwForce, ForceMode.Impulse);
                        }
                        inventoryHolder.InventorySystem.RemoveFromInventory();
                    }
                }
            }
        }
    }   
}

