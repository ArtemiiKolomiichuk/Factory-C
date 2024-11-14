using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    
    [SerializeField]
    private GameObject customerGameObject;

    [SerializeField]
    private Transform spawnPoint;

    private Queue<GameObject> customers = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCustomer() {
        GameObject instantiatedCustomer = Instantiate(customerGameObject, gameObject.transform);
        instantiatedCustomer.transform.SetLocalPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        customers.Enqueue(instantiatedCustomer);
    }

    public void GoHomeCustomer() {
        if(customers == null || customers.Count() == 0) {
            return;
        }
        GameObject customer = customers.Dequeue();
        CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();
        customerMovement.GoToPointThenDestroy(spawnPoint.position);
    }
}
