using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    
    [SerializeField]
    private GameObject customerGameObject;

    [SerializeField]
    private Transform spawnPoint;
    private List<(GameObject customer, Order order)> customersWithOrders;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        customersWithOrders = new ();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetSpawnPoint() {
        return spawnPoint;
    }

    public void SpawnCustomer(Order order = null) {
        GameObject instantiatedCustomer = Instantiate(customerGameObject, gameObject.transform);
        instantiatedCustomer.transform.SetLocalPositionAndRotation(spawnPoint.localPosition, spawnPoint.localRotation);
        //instantiatedCustomer.GetComponent<NetworkObject>().Spawn();
        instantiatedCustomer.GetComponent<Customer>().SetResource(order.resource);
        customersWithOrders.Add((instantiatedCustomer, order));
    }

    public void GoHomeCustomer(Order order = null) {
        int index = 0;
        for(; index < customersWithOrders.Count(); index++) {
            if(customersWithOrders[index].order == order) {
                break;
            }
        }
        if(index < customersWithOrders.Count()) {
            GameObject customer = customersWithOrders[index].customer;
            Customer customerScript = customer.GetComponent<Customer>();
            customerScript.GoHome();
            customersWithOrders.RemoveAt(index);
        }
    }

    public void RemoveCustomer(GameObject customer) {
        int index = 0;
        for(; index < customersWithOrders.Count(); index++) {
            if(customersWithOrders[index].customer == customer) {
                break;
            }
        }
        Debug.Log(index);
        if(index < customersWithOrders.Count()) {
            OrderController.Instance.CompleteOrder(customersWithOrders[index].order);
            customersWithOrders.RemoveAt(index);
        }
    }
}
