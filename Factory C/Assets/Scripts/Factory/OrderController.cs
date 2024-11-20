using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

public class OrderController : MonoBehaviour
{
    public static OrderController Instance { get; private set; }
    
    [SerializeField]
    private uint maxOrderCountAtFirstDay;
    [SerializeField]
    private uint orderCountDelta;
    [SerializeField]
    private float timeBetweenOrdersInSeconds;
    [SerializeField]
    private List<Order> possibleOrders;
    [SerializeField]
    private TextMeshProUGUI orderInfoUI;
    private List<(Order order, uint endTimeInGameMinutes)> currentOrders;
    private uint currentMaxOrderCount;
    private float currentTimeBetweenOrders;

    private uint allCompletedOrders = 0;
    private uint completedOrdersInDay = 0;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentOrders = new ();
        currentMaxOrderCount = maxOrderCountAtFirstDay;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DayController.Instance.DayStoppedAction += OnDayStopped;
        DayController.Instance.NextDayAction += OnNextDay;
        GameController.Instance.GameOverAction += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if(DayController.Instance.IsDayStopped() || GameController.Instance.IsGameOver()) return;
        CheckOrdersOverdue();
        if(currentOrders.Count() < currentMaxOrderCount) {
            currentTimeBetweenOrders += Time.deltaTime;
            if(currentTimeBetweenOrders > timeBetweenOrdersInSeconds) {
                currentTimeBetweenOrders = 0;
                CreateOrder();
                UpdateUI();
            }
        }
    }

    private void CheckOrdersOverdue() {
        for(int i = 0; i < currentOrders.Count(); i++) {
            (Order order, uint endTimeInGameMinutes) = currentOrders[i];
            if(DayController.Instance.GetCurrentDayTime() >= endTimeInGameMinutes) {
                currentOrders.RemoveAt(i);
                CustomerSpawner.Instance.GoHomeCustomer(order);
                ComplainController.Instance.Complain();
                UpdateUI();
                return;
            }
        }
    }

    public void CompleteOrder(Order order) {
        int index = 0;
        for(; index < currentOrders.Count(); index++) {
            if(currentOrders[index].order == order) {
                break;
            }
        }
        if(index < currentOrders.Count()) {
            currentOrders.RemoveAt(index);
            allCompletedOrders++;
            completedOrdersInDay++;
            NotifyPlayersAboutCompleteOrder();
            UpdateUI();
        }
    }

    private void CreateOrder() {
        Order order = GetRandomOrder();
        if(order == null && currentOrders.Count() > 0) {
            DayController.Instance.StopDay();
            return;
        }
        else if (order == null) {
            return;
        }
        uint orderEndTime = DayController.Instance.GetCurrentDayTime() + order.durationInGameMinutes;
        currentOrders.Add((order, orderEndTime));
        NotifyPlayersAboutNewOrder();
        UpdateUI();
        CustomerSpawner.Instance.SpawnCustomer(order);
    }

    private Order GetRandomOrder() {
        List<Order> filteredOrders = FilterOrders();
        if(filteredOrders == null || filteredOrders.Count() == 0) {
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, filteredOrders.Count());
        return filteredOrders[randomIndex];
    }

    private List<Order> FilterOrders() {
        uint possibleOrderTime = DayController.Instance.GetMaxDayTime() - DayController.Instance.GetCurrentDayTime();
        Debug.Log(possibleOrderTime);
        List<Order> filteredOrders = new ();
        foreach(Order order in possibleOrders) {
            if(order.durationInGameMinutes <= possibleOrderTime) {
                filteredOrders.Add(order);
            }
        }
        return filteredOrders;
    }

    private void OnDayStopped() {
        for(int i = 0; i < currentOrders.Count(); i++) {
            (Order order, uint _) = currentOrders[i];
            CustomerSpawner.Instance.GoHomeCustomer(order);
        }
        currentOrders = new ();
        UpdateUI();
    }

    private void OnGameOver() {
        for(int i = 0; i < currentOrders.Count(); i++) {
            (Order order, uint _) = currentOrders[i];
            CustomerSpawner.Instance.GoHomeCustomer(order);
        }
        currentOrders = new ();
        UpdateUI();
    }

    private void OnNextDay() {
        completedOrdersInDay = 0;
    }

    public uint GetAllCompletedOrders() {
        return allCompletedOrders;
    }

    public uint GetCompletedOrdersInDay() {
        return completedOrdersInDay;
    }

    private void NotifyPlayersAboutNewOrder() {
        NotificationController.Instance.AddNotification("New order!", Color.white);
    }

    private void NotifyPlayersAboutCompleteOrder() {
        NotificationController.Instance.AddNotification("Order completed!", Color.white);
    }

    private void UpdateUI() {
        orderInfoUI.text = $"Orders: {currentOrders.Count()}";
    }

    
}
