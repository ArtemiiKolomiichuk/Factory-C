using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{

    public static CustomerGenerator Instance { get; private set; }

    [SerializeField]
    private List<GameObject> possibleCustomers;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Customer GenerateCustomer(Order order) {
        throw new NotImplementedException();
    }
}
