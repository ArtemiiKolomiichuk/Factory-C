using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class NextDayUIController : MonoBehaviour
{

    public static NextDayUIController Instance { get; private set; }

    [SerializeField]
    private Canvas dayStoppedCanvas;
    [SerializeField]
    private TextMeshProUGUI dayInfoUI;
    [SerializeField]
    private TextMeshProUGUI orderInfoUI;
    [SerializeField]
    private TextMeshProUGUI complaintsInfoUI;
    [SerializeField]
    private Button nextDayButton;

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
        DayController.Instance.DayStoppedAction += OnDayStopped;
        dayStoppedCanvas.enabled = false;
        nextDayButton.onClick.AddListener(OnNextDayButon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNextDayButon() {
        dayStoppedCanvas.enabled = false;
        DayController.Instance.NextDay();
    }

    public void OnDayStopped() {
        dayInfoUI.text = $"Day: {DayController.Instance.GetCurrentDay()}";
        orderInfoUI.text = $"Orders completed: {OrderController.Instance.GetCompletedOrdersInDay()}";
        complaintsInfoUI.text = $"Complaints got: {ComplainController.Instance.GetComplaints()}";
        dayStoppedCanvas.enabled = true;
        if(NetworkManager.Singleton.IsServer) {
            nextDayButton.enabled = true;
        }
        else {
            nextDayButton.enabled = false;
        }
    }
}
