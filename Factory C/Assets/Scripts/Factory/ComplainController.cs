using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ComplainController : NetworkBehaviour
{

    public static ComplainController Instance { get; private set; }
    [SerializeField]
    private uint maxComplaintCount;
    [SerializeField]
    private uint deltaComplainCount;
    [SerializeField]
    private TextMeshProUGUI complaintsInfoUI;
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
        if(!NetworkManager.Singleton.IsHost) return;
        UpdateUI();
        DayController.Instance.NextDayAction += OnNextDay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public uint GetComplaints(){
        return currentComplaintCount;
    }

    public void Complain() {
        currentComplaintCount++;
        UpdateCurrentComplaintCountClientRpc(currentComplaintCount);
        UpdateUI();
        NotifyPlayersAboutComplain();
        if(currentComplaintCount >= maxComplaintCount) {
            GameController.Instance.GameOver();
        }
    }

    [ClientRpc]
    private void UpdateCurrentComplaintCountClientRpc(uint currentComplaintCount) {
        this.currentComplaintCount = currentComplaintCount;
    }

    public void OnNextDay() {
        if(currentComplaintCount > 0) {
            currentComplaintCount -= deltaComplainCount;
            UpdateCurrentComplaintCountClientRpc(currentComplaintCount);
            UpdateUI();
        }
    }

    private void UpdateUI() {
        complaintsInfoUI.text = $"Complains: {currentComplaintCount}/{maxComplaintCount}";
        UpdateUIClientRpc(currentComplaintCount, maxComplaintCount);
    }

    [ClientRpc]
    private void UpdateUIClientRpc(uint currentComplaintCount, uint maxComplaintCount) {
        complaintsInfoUI.text = $"Complains: {currentComplaintCount}/{maxComplaintCount}";
    }

    private void NotifyPlayersAboutComplain() {
        //NotificationController.Instance.AddNotification("You`ve got a complain!", Color.red);
        NotifyPlayersAboutComplainClientRpc();
    }

    [ClientRpc]
    private void NotifyPlayersAboutComplainClientRpc() {
        NotificationController.Instance.AddNotification("You`ve got a complain!", Color.red);
    }

}
