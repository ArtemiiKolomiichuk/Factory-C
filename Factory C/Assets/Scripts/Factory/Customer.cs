using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private Order order;
    private Time orderStartTime;
    private Time estimatedOrderEndTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeResource(Resource resource) {
        throw new NotImplementedException();
    }

    private void CompleteOrder() {
        throw new NotImplementedException();
    }

    private void CheckOrderOverdue() {
        throw new NotImplementedException();
    }

    private void Complain() {
        throw new NotImplementedException();
    }
}
