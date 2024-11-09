using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    
    // Start is called before the first frame update
    public uint maxOrderCountAtMoment;
    private List<Order> currentOrders;

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
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteOrder(Order order) {
        throw new NotImplementedException();
    }

    public void CloseOrder(Order order) {
        throw new NotImplementedException();
    }

    private void TryToCreateOrder() {
        throw new NotImplementedException();
    }

    private void CreateOrder() {
        throw new NotImplementedException();
    }

    private void CreateCustomer() {
        throw new NotImplementedException();
    }

    private void NotifyPlayerAboutOrder(Order order) {
        throw new NotImplementedException();
    }

    
}
