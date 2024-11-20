using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventoryHolder : NetworkBehaviour
{
    [SerializeField]
    private int inventorySize;
    [SerializeField]
    protected InventorySystem inventorySystem;
    public Vector3 offset = new Vector3(0,1, 0);

    public List<GameObject> skins;
    public int currentSkinIndex = 0;

    public GameObject currentShowItem;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {

        inventorySystem = new InventorySystem(inventorySize);
        inventorySystem.OnInventoryUpdated += HandleInventoryUpdate;
        
    }

    //private void Start()
    //{
    //    StartCoroutine(ExampleCoroutine());
    //}
    //IEnumerator ExampleCoroutine()
    //{

    //   yield return new WaitForSeconds(10);
    //   ChangeSkin(1);


    //}
    private void OnDestroy()
    {
        inventorySystem.OnInventoryUpdated -= HandleInventoryUpdate;
        
    }


    private void HandleInventoryUpdate(Resource itemInfo)
    {
        Debug.Log($"Inventory updated: {IsOwner}, {currentShowItem}, {itemInfo}");
        if(NetworkCompanion.networkEnabled && !IsOwner)
        {
            return;
        }
        if (itemInfo != null)
        {
            int index = PrefabSystem.GetIndex(itemInfo);
            UpdateInventoryOnServerRpc(index, new (this), new(this), transform.rotation.eulerAngles, OwnerClientId);
        }
        else if (itemInfo == null)
        {
            TryDestroyMyCurrentItemRpc(new(this));
            currentShowItem = null;
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void TryDestroyMyCurrentItemRpc(NetworkBehaviourReference player)
    {
        if(player.TryGet(out NetworkBehaviour nb))
        {
            if(nb.GetComponent<InventoryHolder>().currentShowItem != null)
            {
                nb.GetComponent<InventoryHolder>().currentShowItem.GetComponent<NetworkObject>().Despawn();
                nb.GetComponent<InventoryHolder>().currentShowItem = null;
            }
        }
        else
        {
            Debug.LogError("NetworkBehaviour not found!!!! #4");
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void UpdateInventoryOnServerRpc(int itemIndex, NetworkBehaviourReference player, NetworkBehaviourReference playerInv, Vector3 rotation, ulong playerId)
    {
        if (playerInv.TryGet(out NetworkBehaviour networkBehaviour))
        {
            if(networkBehaviour.GetComponent<InventoryHolder>().currentShowItem != null)
            {
                return;
            }
            var item = PrefabSystem.GetByIndex(itemIndex);
            item = Instantiate(item, offset, Quaternion.Euler(rotation));
            item.GetComponent<NetworkObject>().Spawn();
            if (player.TryGet(out NetworkBehaviour playerObject))
            {
                item.transform.SetParent(playerObject.transform);
            }
            item.transform.localPosition = offset;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<SphereCollider>().radius = 0.0001f;
            item.GetComponent<SphereCollider>().isTrigger = false;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<NetworkObject>().ChangeOwnership(playerId);
            networkBehaviour.GetComponent<InventoryHolder>().currentShowItem = item;
        }
        
    }

}
