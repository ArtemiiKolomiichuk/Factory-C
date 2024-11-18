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

    private GameObject currentShowItem;

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
        if(NetworkCompanion.networkEnabled && !IsOwner)
        {
            return;
        }

        if (currentShowItem == null && itemInfo != null)
        {
            GameObject item = PrefabSystem.FindItem(itemInfo);
            currentShowItem = Instantiate(item, transform.position + offset, transform.rotation);
            NetworkBehaviour nb = currentShowItem.GetComponent<NetworkBehaviour>();
            if (nb)
            {
                if(!nb.IsSpawned && IsHost)
                {
                    nb.GetComponent<NetworkObject>().Spawn();
                }
            }
            currentShowItem.transform.SetParent(transform);
            currentShowItem.GetComponent<Rigidbody>().isKinematic = true;
            currentShowItem.GetComponent<SphereCollider>().radius = 0.0001f;
            currentShowItem.GetComponent<SphereCollider>().isTrigger = false;
            currentShowItem.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (currentShowItem != null && itemInfo == null)
        {
            Destroy(currentShowItem);
            currentShowItem = null;
        }
    }
    //public void ChangeSkin(int skinIndex)
    //{
    //    if (skinIndex >= 0 && skinIndex < skins.Count)
    //    {
    //        ApplySkin(skinIndex);
    //    }
    //}

    //private void ApplySkin(int index)
    //{
    //    if (skins[index] != null)
    //    {
    //        skins[currentSkinIndex].SetActive(false);
    //        skins[index].SetActive(true);
    //        currentSkinIndex = index;
    //    }
        
    //}





}
