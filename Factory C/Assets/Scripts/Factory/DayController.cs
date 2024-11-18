using System;
using UnityEngine;
using TMPro;

public class DayController : MonoBehaviour
{

    public static DayController Instance { get; private set; }
    [SerializeField]
    private float gameMinuteDurationInSeconds;
    [SerializeField]
    private uint hoursInDay;
    [SerializeField]
    private uint minutesInHour;
    [SerializeField]
    private TextMeshProUGUI dayInfoUI;
    private float currentAccumulatedSeconds = 0;
    private uint currentDay = 1;
    private uint currentDayTimeInGameMinutes = 0;

    private bool isDayStopped = false;

    public Action DayStoppedAction;
    public Action NextDayAction;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(isDayStopped || GameController.Instance.IsGameOver()) return;
        currentAccumulatedSeconds += Time.deltaTime;
        //Debug.Log($"{currentAccumulatedSeconds} {gameMinuteDurationInSeconds} {currentDayTimeInGameMinutes}");
        if(currentAccumulatedSeconds > gameMinuteDurationInSeconds) {
            currentAccumulatedSeconds -= gameMinuteDurationInSeconds;
            IncreaseCurrentDayTime();
        }
    }

    public uint GetCurrentDay() {
        return currentDay;
    }

    public uint GetCurrentDayTime() {
        return currentDayTimeInGameMinutes;
    }

    public uint GetMaxDayTime() {
        return hoursInDay * minutesInHour - 1;
    }

    private void IncreaseCurrentDayTime() {
        currentDayTimeInGameMinutes++;
        UpdateUI();
        if(currentDayTimeInGameMinutes >= hoursInDay * minutesInHour) {
            StopDay();
        }
    }

    private void UpdateUI() {
        string dayInfo = $"Day: {currentDay} ";
        uint currentHours = currentDayTimeInGameMinutes / minutesInHour;
        uint currentHourMinutes = currentDayTimeInGameMinutes - (currentHours * minutesInHour);
        dayInfo += currentHours < 10 ? $"0{currentHours}:" : $"{currentHours}:";
        dayInfo += currentHourMinutes < 10 ? $"0{currentHourMinutes}" : $"{currentHourMinutes}";
        //Debug.Log(dayInfo);
        dayInfoUI.text = dayInfo;
    }

    public bool IsDayStopped() {
        return isDayStopped;
    }

    public void StopDay() {
        if(!isDayStopped) {
            DayStoppedAction?.Invoke();
        }
        currentAccumulatedSeconds = 0;
        isDayStopped = true;
    }

    public void NextDay() {
        currentDay++;
        currentDayTimeInGameMinutes = 0;
        UpdateUI();
        isDayStopped = false;
        NextDayAction?.Invoke();
    }

}
