using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : NetworkBehaviour
{
    public float PickUpRadius = 0.6f;
    public Resource ItemData;

    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = PickUpRadius;
    }

    public override void OnDestroy()
    {
        Debug.Log($"ItemPickUp OnDestroy {{{name}}}");
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<InventoryHolder>();
        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData))
        {
            DestroyOnAddingRpc(gameObject.GetComponent<NetworkBehaviour>());
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void DestroyOnAddingRpc(NetworkBehaviourReference networkBehaviourReference)
    {
        if (networkBehaviourReference.TryGet(out NetworkBehaviour networkBehaviour))
        {
            networkBehaviour.NetworkObject.Despawn(true);
        }
        else
        {
            Debug.LogError("NetworkBehaviour not found!!!! #5");
        }
    }

}
