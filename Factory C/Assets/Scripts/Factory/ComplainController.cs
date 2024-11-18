using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComplainController : MonoBehaviour
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
        UpdateUI();
        DayController.Instance.NextDayAction += OnNextDay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Complain() {
        currentComplaintCount++;
        UpdateUI();
        NotifyPlayersAboutComplain();
        if(currentComplaintCount >= maxComplaintCount) {
            GameController.Instance.GameOver();
        }
    }

    public void OnNextDay() {
        currentComplaintCount -= deltaComplainCount;
        UpdateUI();
    }

    private void UpdateUI() {
        complaintsInfoUI.text = $"Complains: {currentComplaintCount}/{maxComplaintCount}";
    }

    private void NotifyPlayersAboutComplain() {
        NotificationController.Instance.AddNotification("You`ve got a complain!", Color.red);
    }
}
