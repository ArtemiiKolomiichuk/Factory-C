using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



    [RequireComponent(typeof(BoxCollider))]
    public class Interactable : MonoBehaviour, UniverslaResourceHolderInterface
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
                TryPlaceItem();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                TryRetrieveItem();
            }
        }

        private void TryPlaceItem()
        {
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
        }

        private void TryRetrieveItem()
        {
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
                Destroy(showObject);
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
