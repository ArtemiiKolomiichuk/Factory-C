using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : NetworkBehaviour, UniverslaResourceHolderInterface
{
    private BoxCollider boxCollider;
    private InventoryHolder inventoryHolder;
    private Resource currentItem;
    private GameObject showObject;
    private bool itemPlaced = false;
    public Vector3 offset = new Vector3(0, 1, 1);

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPlaceItemRpc(new(inventoryHolder.GetComponent<NetworkBehaviour>()));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            TryRetrieveItemRpc(new(inventoryHolder.GetComponent<NetworkBehaviour>()));
        }
    }

    [Rpc(SendTo.Server)]
    private void TryPlaceItemRpc(NetworkBehaviourReference inv)
    {
        if(inv.TryGet(out NetworkBehaviour nb)){
            inventoryHolder = nb.GetComponent<InventoryHolder>();
        }
        if (inventoryHolder != null && !itemPlaced)
        {
            var itemData = inventoryHolder.InventorySystem.GetInfo();
            if (itemData != null)
            {
                PutResource(itemData);
                inventoryHolder.InventorySystem.RemoveFromInventory();
                DisplayItem();
                itemPlaced = true;
            }
        }
        else
        {
            Debug.Log($"itemPlaced: {itemPlaced}");
        }
    }

    [Rpc(SendTo.Server)]
    private void TryRetrieveItemRpc(NetworkBehaviourReference inv)
    {
        if (inv.TryGet(out NetworkBehaviour nb))
        {
            inventoryHolder = nb.GetComponent<InventoryHolder>();
        }
        if (inventoryHolder != null && itemPlaced)
        {
            var itemData = inventoryHolder.InventorySystem.GetInfo();
            if (itemData == null && HaveResource())
            {
                inventoryHolder.InventorySystem.AddToInventory(PullResource());
                RemoveDisplayedItem();
                itemPlaced = false;
            }
        }
    }

    private void DisplayItem()
    {
        if (currentItem != null && showObject == null)
        {
            GameObject itemPrefab = PrefabSystem.FindItem(currentItem);
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

    private void RemoveDisplayedItem()
    {
        if (showObject != null)
        {
            showObject.GetComponent<NetworkObject>().Despawn(true);
            showObject = null;
        }
        ClearResources();
    }

    private void OnTriggerEnter(Collider other)
    {
        var holder = other.GetComponent<InventoryHolder>();
        if (holder != null)
        {
            inventoryHolder = holder;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var holder = other.GetComponent<InventoryHolder>();
        if (holder != null)
        {
            inventoryHolder = null;
        }
    }
    public void ClearResources()
    {
        currentItem = null;
    }

    public bool HaveFreeSpace()
    {
        return currentItem == null;
    }
    public int GetFreeSpaceCounter()
    {
        return currentItem == null ? 1 : 0;
    }

    public bool HaveResource()
    {
        return currentItem != null;
    }

    public int GetCountOfResources()
    {
        return currentItem != null ? 1 : 0;
    }

    public void PutResourceType(ResourceType resourceType)
    {
        if (resourceType != ResourceType.None)
        {
            currentItem = ResourceController.Instance.resourceDictionary[resourceType];
        }
    }

    public void PutResource(Resource resource)
    {
        currentItem = resource;
    }

    public ResourceType PullResourceType()
    {
        return currentItem != null ? currentItem.rType : ResourceType.None;
    }

    public Resource PullResource()
    {
        var resource = currentItem;
        ClearResources();
        return resource;
    }
}
