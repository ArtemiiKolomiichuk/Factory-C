using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplainController : MonoBehaviour
{

    public static ComplainController Instance { get; private set; }
    [SerializeField]
    private uint maxComplaintCount;
    private uint currentComplaintCount;

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

    public void Complain() {
        throw new NotImplementedException();
    }

    private void NextDay() {
        throw new NotImplementedException();
    }

    private void CheckComplaints() {
        throw new NotImplementedException();
    }
}
