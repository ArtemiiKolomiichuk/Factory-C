using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Table : NetworkBehaviour
{
    public int? itemIndex;
    public GameObject showObject;
    public Vector3 offset;
    public int tableIndex;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        other.GetComponent<PlayerMovement3D>().TableSubscribe(this);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        other.GetComponent<PlayerMovement3D>().TableUnsubscribe(this);
    }

    public void TakeItem(PlayerMovement3D player)
    {
        TakeItemServerRpc(new(player.GetComponent<NetworkBehaviour>()), tableIndex);
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    void TakeItemServerRpc(NetworkBehaviourReference player, int table)
    {
            if(player.TryGet(out NetworkBehaviour pl))
            {
                var tables = FindObjectsOfType<Table>();
                var tb = tables.Where(t => t.tableIndex == table).FirstOrDefault();
            if (tb.GetComponent<Table>().itemIndex is int index)
                {
                    if(pl.GetComponent<InventoryHolder>().currentShowItem == null)
                    {
                        pl.GetComponent<InventoryHolder>().InventorySystem.AddToInventory(PrefabSystem.GetByIndex(index).GetComponent<ItemPickUp>().ItemData);
                        showObject.GetComponent<NetworkObject>().Despawn(true);
                        itemIndex = null;
                    }
                }
            }
        
    }

    public void PutItem(PlayerMovement3D player)
    {
        var nbr1 = new NetworkBehaviourReference(player.GetComponent<NetworkBehaviour>());
        PutItemServerRpc(nbr1, tableIndex);
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    void PutItemServerRpc(NetworkBehaviourReference player, int table)
    {
        if (player.TryGet(out NetworkBehaviour pl))
        {
            var tables = FindObjectsOfType<Table>();
            var tb = tables.Where(t => t.tableIndex == table).FirstOrDefault();
            if (tb.GetComponent<Table>().itemIndex is null)
            {
                if (pl.GetComponent<InventoryHolder>().currentShowItem != null)
                {
                    int index = PrefabSystem.GetIndex(pl.GetComponent<InventoryHolder>().currentShowItem.GetComponent<ItemPickUp>().ItemData);
                    if (pl.GetComponent<InventoryHolder>().InventorySystem.RemoveFromInventory())
                    {
                        itemIndex = index;
                        Debug.Log($"Item removed from inventory {index}");
                        GameObject itemPrefab = PrefabSystem.GetByIndex(index);
                        showObject = Instantiate(itemPrefab, transform.position + offset, transform.rotation);
                        showObject.GetComponent<NetworkObject>().Spawn();
                        var rb = showObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                            rb.useGravity = false;
                        }
                        var collider = showObject.GetComponent<Collider>();
                        if (collider != null)
                        {
                            collider.isTrigger = false;
                        }
                    }
                }
            }
        }
        
    }
}
