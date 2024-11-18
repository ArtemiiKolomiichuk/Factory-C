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
        base.OnDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<InventoryHolder>();
        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData))
        {
            Destroy(gameObject);
        }
    }

}
