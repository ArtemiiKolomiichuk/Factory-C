using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    public static FactoryController Instance { get; private set; }
    
    //I don't which type to use
    private object customerZone;
    //I don't which type to use
    private object customerEnter;

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

    public void GetCustomerZone() {
        throw new NotImplementedException();
    }

    public void GetCustomerEnter() {
        throw new NotImplementedException();
    }
}
