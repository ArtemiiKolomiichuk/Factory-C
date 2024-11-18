using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerTemperatureControlMinigame : MinigameInterface
{
    [Header("Temperature Control Minigame Settings")]
    public GameObject temperatureBar;

    public Button playerProceedButton;
    public Button playerHeatButton;
    public Button playerCoolButton;

    public Image playerProceedIndicator;
    private Color playerProceedIndicatorBaseColor;

    public Image playerHeatIndicator;
    private Color playerHeatIndicatorBaseColor;

    public Image playerCoolIndicator;
    private Color playerCoolIndicatorBaseColor;

    public float allowedZoneSize = 100;
    public Image allowedZoneImage;

    private static readonly float TEMPERATURE_DECAY_RATE = 0.1f;

    public float fullBaseSize = 1000;
    public RectTransform fullTemperatureBar = null;
    private float fullTemperatureBarSize;
    private float pointScaler = 1;

    private float currentTemperature = 0f;

    private float minTemperature = -500f;
    private float maxTemperature = 500f;
    
    public float temperatureChangeSpeed = 5f;

    private bool isSynchronize = false;

    private bool player1PressedHeat = false;
    private bool player1PressedCool = false;
    private bool player2PressedHeat = false;
    private bool player2PressedCool = false;


    private float temperatureTargetLowerLimit; //SET LATER
    private float temperatureTargetUpperLimit; //SET LATER

    protected override void Awake()
    {
        corespondingWorkstationType = WorkstationType.Forge;
        base.Awake();
    }

    protected override void Start()
    {
        playerProceedIndicatorBaseColor = playerProceedIndicator.color;
        playerHeatIndicatorBaseColor = playerHeatIndicator.color;
        playerCoolIndicatorBaseColor = playerCoolIndicator.color;

        fullTemperatureBarSize = fullTemperatureBar.rect.width;
        pointScaler = fullBaseSize / fullTemperatureBarSize;

        minTemperature = -fullBaseSize / 2;
        maxTemperature = fullBaseSize / 2;

        temperatureTargetLowerLimit = minTemperature + (allowedZoneSize / 2);
        temperatureTargetUpperLimit = maxTemperature - (allowedZoneSize / 2);

        playerHeatButton.onClick.AddListener(() => OnPlayer1HotButtonPress());
        playerCoolButton.onClick.AddListener(() => OnPlayer1ColdButtonPress());


        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        UpdateTemperature();
        UpdateProgressBar();
    }

    private void UpdateTemperature()
    {
        if (isSynchronize)
        {
            if (player1PressedHeat && player2PressedHeat)
            {
                currentTemperature += temperatureChangeSpeed * Time.deltaTime;
            }
            else if (player1PressedCool && player2PressedCool)
            {
                currentTemperature -= temperatureChangeSpeed * Time.deltaTime;
            }

            ////////
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
            if (player1PressedHeat && player2PressedHeat || player1PressedCool && player2PressedCool)
            {
                progressCount += 1;
                ChangeProgressBar(progressCount);
                Debug.Log($"Progress Step {progressCount}");

                if (progressCount >= targetProgressCount) 
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
        player1PressedHeat = true;
        CheckSyncStatus();
    }

    private void OnPlayer1ColdButtonPress()
    {
        player1PressedCool = true;
        CheckSyncStatus();
    }

    private void OnPlayer2HotButtonPress()
    {
        player2PressedHeat = true;
        CheckSyncStatus();
    }

    private void OnPlayer2ColdButtonPress()
    {
        player2PressedCool = true;
        CheckSyncStatus();
    }

    private void CheckSyncStatus()
    {
        if ((player1PressedHeat && player2PressedHeat) || (player1PressedCool && player2PressedCool))
        {
            if (!isSynchronize)
            {
                isSynchronize = true;
                Debug.Log("Players are in sync!");
            }
        }
        else
        {
            isSynchronize = false;
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
        player1PressedHeat = false;
        player1PressedCool = false;
        player2PressedHeat = false;
        player2PressedCool = false;
        isSynchronize = false;
    }
}
