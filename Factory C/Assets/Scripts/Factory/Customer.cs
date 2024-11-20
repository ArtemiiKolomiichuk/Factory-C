using Unity.Netcode;
using UnityEngine;

public class Customer : NetworkBehaviour
{
    [SerializeField]
    private string interactionHint;
    [SerializeField]
    private Vector3 offset = new Vector3(0, 20, 0);
    private Resource resource;
    private CustomerState state = CustomerState.WAITING;
    private Outline outline;
    private CustomerMovement customerMovement;
    private InventoryHolder playerInventoryHolder;
    private GameObject instantiatedResource;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        if(outline)
            outline.enabled = false;
        customerMovement = GetComponent<CustomerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerInventoryHolder != null) {
            playerInventoryHolder.InventorySystem.RemoveFromInventory();
            CompleteOrder();
        }
    }

    public void SetResource(Resource resource) {
        this.resource = resource;
        GameObject item = PrefabSystem.FindItem(resource);
        Debug.Log(item == null);
        instantiatedResource = Instantiate(item, transform.position + offset, transform.rotation);
        instantiatedResource.GetComponent<NetworkObject>().Spawn();
        instantiatedResource.transform.SetParent(transform);
        //instantiatedResource.transform.localScale *= 4;
        instantiatedResource.GetComponent<Rigidbody>().isKinematic = true;
        instantiatedResource.GetComponent<SphereCollider>().radius = 0.0001f;
        instantiatedResource.GetComponent<SphereCollider>().isTrigger = false;
        instantiatedResource.GetComponent<Rigidbody>().useGravity = false;
    }

    public void GoHome() {
        SetState(CustomerState.GO_HOME);
    }

    private void CompleteOrder() {
        CustomerSpawner.Instance.RemoveCustomer(this.gameObject);
        SetState(CustomerState.GO_HOME);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state == CustomerState.GO_HOME) {
            return;
        }
        if(!other.CompareTag("Player")) {
            return;
        }
        playerInventoryHolder = other.GetComponent<InventoryHolder>();
        /*if(resource != inventoryHolder.InventorySystem.GetInfo()) {
            inventoryHolder = null;
            return;
        }*/
        outline.enabled = true;
        InteractableCanvas.Instance.ShowHint(interactionHint);
        SetState(CustomerState.INTERACT_WITH_PLAYER);
    }

    private void SetState(CustomerState state) {
        this.state = state;
        customerMovement.SetState(state);
        if(this.state == CustomerState.GO_HOME) {
            this.playerInventoryHolder = null;
            outline.enabled = false;
            InteractableCanvas.Instance.HideHint();
            if(instantiatedResource != null) {
                Destroy(instantiatedResource);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(state == CustomerState.GO_HOME) {
            return;
        }
        if(!other.CompareTag("Player")) {
            return;
        }
        playerInventoryHolder = null;
        outline.enabled = false;
        InteractableCanvas.Instance.HideHint();
        SetState(CustomerState.WAITING);
    }
}

public enum CustomerState {
    WAITING,
    INTERACT_WITH_PLAYER,
    GO_HOME,
}