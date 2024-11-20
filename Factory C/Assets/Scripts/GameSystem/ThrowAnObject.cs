using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ThrowAnObject : NetworkBehaviour
{
    public GameObject player;
    public float throwForce = 3f;
    public Vector3 throwOffset = new Vector3(0, 1, 1);

    private void Start()
    {
        if(player == null)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && IsOwner)
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
                        ThrowObjectRpc(PrefabSystem.GetIndex(item), player.transform.position + player.transform.TransformDirection(throwOffset), player.transform.rotation.eulerAngles, player.transform.forward * throwForce);
                        inventoryHolder.InventorySystem.RemoveFromInventory();
                    }
                }
            }
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void ThrowObjectRpc(int index, Vector3 position, Vector3 rotation, Vector3 rbVec)
    {
        var item = PrefabSystem.GetByIndex(index);
        item = Instantiate(item, position, Quaternion.Euler(rotation));
        item.GetComponent<NetworkObject>().Spawn();
        item.GetComponent<Rigidbody>().AddForce(rbVec, ForceMode.Impulse);
    }
}

