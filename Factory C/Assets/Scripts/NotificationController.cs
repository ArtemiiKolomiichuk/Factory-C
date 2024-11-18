using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NotificationController : MonoBehaviour
{

    public static NotificationController Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI notificationInfoUI;
    [SerializeField]
    private float notificationTime;
    private float currentNotificationTime;

    private Queue<(String notification, Color color)> notifications;
    Animator animator;

    private bool isAnimation = false;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        notifications = new ();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = notificationInfoUI.GetComponent<Animator>();
        notificationInfoUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimation) {
            currentNotificationTime += Time.deltaTime;
            if(currentNotificationTime > notificationTime) {
                currentNotificationTime = 0;
                isAnimation = false;
                notificationInfoUI.enabled = false;
            }
        }
        else if(notifications.Count() > 0) {
            isAnimation = true;
            currentNotificationTime = 0;
            (String notification, Color color) = notifications.Dequeue();
            notificationInfoUI.enabled = true;
            notificationInfoUI.text = notification;
            notificationInfoUI.color = color;
            animator.Play("NotificationAnimation", -1, 0f);
        }
    }

    public void AddNotification(String notification, Color color) {
        notifications.Enqueue((notification, color));
    }
}
