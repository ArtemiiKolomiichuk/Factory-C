using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerTemperatureControlMinigame : MinigameInterface
{
    [Header("Temperature Control Minigame Settings")]
    public GameObject temperatureBar;
    public GameObject progressBar;

    public Button player1HotButton;
    public Button player1ColdButton;
    public Button player2HotButton;
    public Button player2ColdButton;

    private static readonly float TEMPERATURE_DECAY_RATE = 0.1f;
    private float currentTemperature = 50f;
    private float minTemperature = 20f;
    private float maxTemperature = 100f;
    private float temperatureChangeSpeed = 5f;

    private bool isSyncing = false;

    private bool player1PressedHot = false;
    private bool player1PressedCold = false;
    private bool player2PressedHot = false;
    private bool player2PressedCold = false;


    private float temperatureTargetLowerLimit = 30f;
    private float temperatureTargetUpperLimit = 90f;

    protected override void Start()
    {
        minigameName = "Two Player Temperature Control";
        base.Start();


        player1HotButton.onClick.AddListener(() => OnPlayer1HotButtonPress());
        player1ColdButton.onClick.AddListener(() => OnPlayer1ColdButtonPress());
        player2HotButton.onClick.AddListener(() => OnPlayer2HotButtonPress());
        player2ColdButton.onClick.AddListener(() => OnPlayer2ColdButtonPress());
    }

    protected override void Update()
    {
        base.Update();
        UpdateTemperature();
        UpdateProgressBar();
    }

    private void UpdateTemperature()
    {
        if (isSyncing)
        {
            if (player1PressedHot && player2PressedHot)
            {
                currentTemperature += temperatureChangeSpeed * Time.deltaTime;
            }
            else if (player1PressedCold && player2PressedCold)
            {
                currentTemperature -= temperatureChangeSpeed * Time.deltaTime;
            }

            currentTemperature = Mathf.Clamp(currentTemperature, minTemperature, maxTemperature);


            currentTemperature -= TEMPERATURE_DECAY_RATE * Time.deltaTime;
            currentTemperature = Mathf.Clamp(currentTemperature, minTemperature, maxTemperature);
            

            ChangeTemperatureBar(currentTemperature);
        }
    }

    private void ChangeTemperatureBar(float temperature) { 
        //TODO change
    }

    private void UpdateProgressBar()
    {
        if (currentTemperature >= temperatureTargetLowerLimit && currentTemperature <= temperatureTargetUpperLimit)
        {
            if (player1PressedHot && player2PressedHot || player1PressedCold && player2PressedCold)
            {
                progressCount += 1;
                ChangeProgressBar(progressCount);
                Debug.Log($"Progress Step {progressCount}");

                if (progressCount >= targetProgressCout) 
                {
                    Success();
                }

                //TODO Randomly change target temp TODO
                temperatureTargetLowerLimit = Random.Range(20f, 50f);
                temperatureTargetUpperLimit = Random.Range(50f, 80f);
            }
        }
    }

    private void ChangeProgressBar(float progress)
    {
        //TODO change
    }

    public override void Success()
    {
        base.Success();
        Debug.Log("Two Player Temperature Control: Success!");
    }

    protected override void HandleInput()
    {
        
    }

    private void OnPlayer1HotButtonPress()
    {
        player1PressedHot = true;
        CheckSyncStatus();
    }

    private void OnPlayer1ColdButtonPress()
    {
        player1PressedCold = true;
        CheckSyncStatus();
    }

    private void OnPlayer2HotButtonPress()
    {
        player2PressedHot = true;
        CheckSyncStatus();
    }

    private void OnPlayer2ColdButtonPress()
    {
        player2PressedCold = true;
        CheckSyncStatus();
    }

    private void CheckSyncStatus()
    {
        if ((player1PressedHot && player2PressedHot) || (player1PressedCold && player2PressedCold))
        {
            if (!isSyncing)
            {
                isSyncing = true;
                Debug.Log("Players are in sync!");
            }
        }
        else
        {
            isSyncing = false;
            Debug.Log("Players are out of sync.");
        }
    }

    protected override void OnMinigameOpened()
    {
        base.OnMinigameOpened();
        progressCount = 0;
        ChangeProgressBar(0);
        currentTemperature = 50f;
    }

    protected override void OnMinigameClosed()
    {
        base.OnMinigameClosed();
        
        ResetButtonStates();
    }

    private void ResetButtonStates()
    {
        player1PressedHot = false;
        player1PressedCold = false;
        player2PressedHot = false;
        player2PressedCold = false;
        isSyncing = false;
    }
}
