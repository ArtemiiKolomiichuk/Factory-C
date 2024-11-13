using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ItemsScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class Interactable: MonoBehaviour
    {
        private BoxCollider boxCollider;
        private Collider other;
        public InventoryHolder inventoryHolder;
        private Resource currentItem;
        private GameObject showObject;
        public Vector3 offset = new Vector3(0, 1, 1);


        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && inventoryHolder != null)
            {
                print("Pressed F");
                var itemData = inventoryHolder.InventorySystem.GetInfo();
                if (itemData != null)
                {
                    currentItem = itemData;
                    inventoryHolder.InventorySystem.RemoveFromInventory();
                    if (currentItem != null && showObject == null)
                    {
                        GameObject item = PrefabSystem.FindItem(currentItem);
                        showObject = Instantiate(item, transform.position + offset, transform.rotation);
                        showObject.GetComponent<Rigidbody>().isKinematic = true;
                        showObject.GetComponent<SphereCollider>().isTrigger = false;
                        showObject.GetComponent<Rigidbody>().useGravity = false;
                    }
                }


            }
            if (Input.GetKeyDown(KeyCode.E) && inventoryHolder != null)
            {
                print("Pressed E");
                var itemData = inventoryHolder.InventorySystem.GetInfo();
                if (currentItem != null && itemData == null)
                {
                    inventoryHolder.InventorySystem.AddToInventory(currentItem);
                    currentItem = null;
                    Destroy(showObject);
                    showObject = null;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            this.other = other;
            if (!other.GetComponent<InventoryHolder>()) return;
            inventoryHolder = other.GetComponent<InventoryHolder>();
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<InventoryHolder>())
            {
                inventoryHolder = null;
            }
        }


    }
}
