using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ForgeMButtonT
{
    Heat,
    Proceed,
    Cool
}

public class TwoPlayerTemperatureControlMinigame : MinigameInterface
{
    [Header("Temperature Control Minigame Settings")]
    public ProgressBarScaler temperatureBar;

    public Button playerProceedButton;
    public Button playerHeatButton;
    public Button playerCoolButton;

    public Image playerProceedIndicator;
    private Color playerProceedIndicatorBaseColor;

    public Image playerHeatIndicator;
    private Color playerHeatIndicatorBaseColor;

    public Image playerCoolIndicator;
    private Color playerCoolIndicatorBaseColor;

    public TimeCounterBarController cooldownTimer = null;
    public TimeCounterBarController buttonTimeTimer = null;

    private float passiveTemperatureDecay = 0.1f;

    public float fullBaseSize = 1000;
    public RectTransform fullTemperatureBar = null;
    private float fullTemperatureBarSize;
    private float pointScaler = 1;

    private float baseTemperature = 0f;
    private float currentTemperature = 0f;

    private float minTemperature = -500f;
    private float maxTemperature = 500f;
    
    public float temperatureChangeDelta = 5f;

    private bool onCooldown = false;
    private float timeOfPress = -1;
    public float buttonTime = 1f;
    public float buttonCooldown = 1f; //cooldown is local
    private Coroutine cooldownCoroutine = null;

    private int playerIndex = 0;
    private int otherPlayerIndex = 0;

    private bool[,] playerPressedButton = new bool[3,2];

    public float allowedZoneSize = 100;
    public Image allowedZoneImage;
    private float temperatureTargetLowerLimit;
    private float temperatureTargetUpperLimit;
    private float allowedPoint = 0;

    private int numberOfFails = 0;
    private int maxNumberOfFails = 3;
    

    protected override void Awake()
    {
        corespondingWorkstationType = WorkstationType.Forge;
        base.Awake();
    }

    protected override void Start()
    {
        buttonTimeTimer.maxTime = buttonTime;
        cooldownTimer.maxTime = buttonCooldown;

        playerProceedIndicatorBaseColor = playerProceedIndicator.color;
        playerHeatIndicatorBaseColor = playerHeatIndicator.color;
        playerCoolIndicatorBaseColor = playerCoolIndicator.color;

        fullTemperatureBarSize = fullTemperatureBar.rect.width;
        pointScaler = fullBaseSize / fullTemperatureBarSize;

        minTemperature = -fullBaseSize / 2;
        maxTemperature = fullBaseSize / 2;

        temperatureTargetLowerLimit = minTemperature + (allowedZoneSize / 2);
        temperatureTargetUpperLimit = maxTemperature - (allowedZoneSize / 2);

        playerHeatButton.onClick.AddListener(() => OnPlayerHeatButtonPress());
        playerCoolButton.onClick.AddListener(() => OnPlayerCoolButtonPress());
        playerProceedButton.onClick.AddListener(() => OnPlayerProceedButtonPress());

        

        currentTemperature = baseTemperature;
        ChoseAllowedZone();

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        UpdatePassiveTemperature();
    }

    //maybe not needed
    private void UpdatePassiveTemperature()
    {
        if (currentTemperature > baseTemperature) 
        {
            ChangeTemperature(-passiveTemperatureDecay * Time.deltaTime);
        }
        else
        {
            ChangeTemperature(passiveTemperatureDecay * Time.deltaTime);
        }
    }

    private void ResetFails() 
    {
        numberOfFails = 0;
    }

    private void AddFail() {
        numberOfFails++;
        if (numberOfFails >= maxNumberOfFails) {
            ResetFails();
            Fail();
        }
    }

    private void ChangeTemperature(float delta) 
    {
        currentTemperature += delta;
        if (minTemperature > currentTemperature) 
        { 
            currentTemperature = minTemperature;
            Fail();
            return;
        }
        if (currentTemperature > maxTemperature) 
        {
            currentTemperature = maxTemperature;
            Fail();
            return;
        }

        temperatureBar.ChangeProgressBar(currentTemperature, maxTemperature);

    }

    private void Proceed() 
    {
        if (IsValueInZone(currentTemperature))
        {
            ChangeProgressCount(1);
        }
        else {
            AddFail();
        }
        ChoseAllowedZone();
    }

    private bool IsValueInZone(float value)
    {
        float halfSize = allowedZoneSize / 2;
        float lowerBound = allowedPoint - halfSize;
        float upperBound = allowedPoint + halfSize;

        return lowerBound <= value && value <= upperBound;
    }

    private void ChoseAllowedZone() {
        allowedPoint = Random.Range(temperatureTargetLowerLimit, temperatureTargetUpperLimit);
        MoveAllowedZoneBar();
    }

    private void MoveAllowedZoneBar() 
    {
        Vector3 pos = allowedZoneImage.rectTransform.localPosition;
        pos.y = allowedPoint * pointScaler;
        Debug.Log("FORGE | allowedPoint:" + allowedPoint + " y=" + pos.y);
        allowedZoneImage.rectTransform.localPosition = pos;
    }

    public override void Success()
    {
        base.Success();
        Debug.Log("Two Player Temperature Control: Success!");
    }

    public override void Fail()
    {
        base.Fail();
        Debug.Log("Two Player Temperature Control: Fail!");
    }

    private void StartTimers() 
    {
        onCooldown = true;
        cooldownCoroutine = StartCoroutine(CooldownCoroutine(buttonCooldown));
        cooldownTimer.StartTimer();
        buttonTimeTimer.StartTimer();
    }

    private IEnumerator CooldownCoroutine(float cooldownDuration)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        onCooldown = false;
        cooldownCoroutine = null;
        Debug.Log("Cooldown complete");
    }

    private void ResetTimers() //reset to both players
    {
        onCooldown = false; 
        timeOfPress = -1;
        if (cooldownCoroutine != null) 
        { 
            StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = null;
        }
        cooldownTimer.StopTimer();
        buttonTimeTimer.StopTimer();
    }

    private void OnPlayerHeatButtonPress()
    {
        if (onCooldown)
        {
            return;
        }
        playerPressedButton[(int)ForgeMButtonT.Heat, playerIndex] = true;
        StartTimers();
        if (CheckSyncStatus(ForgeMButtonT.Heat))
        {
            ChangeTemperature(temperatureChangeDelta);
        }
    }

    private void OnPlayerCoolButtonPress()
    {
        if (onCooldown)
        {
            return;
        }
        playerPressedButton[(int)ForgeMButtonT.Cool, playerIndex] = true;
        StartTimers();
        if (CheckSyncStatus(ForgeMButtonT.Cool))
        {
            ChangeTemperature(-temperatureChangeDelta);
        }
    }

    private void OnPlayerProceedButtonPress()
    {
        if (onCooldown)
        {
            return;
        }
        playerPressedButton[(int)ForgeMButtonT.Heat, playerIndex] = true;
        StartTimers();
        if (CheckSyncStatus(ForgeMButtonT.Heat))
        {
            Proceed();
        }
    }

    private float GetTime() {
        return 5; //TODO get time for sync
    }

    private void ChangeColorOfIndicator(bool status, ForgeMButtonT buttonType) {
        switch (buttonType)
        {
            case ForgeMButtonT.Heat:
                SetColorOfIdicator(playerHeatIndicator, playerHeatIndicatorBaseColor, status);
                break;
            case ForgeMButtonT.Proceed:
                SetColorOfIdicator(playerProceedIndicator, playerProceedIndicatorBaseColor, status);
                break;
            case ForgeMButtonT.Cool:
                SetColorOfIdicator(playerCoolIndicator, playerCoolIndicatorBaseColor, status);
                break;
            default:
                break;
        }
    }

    private void SetColorOfIdicator(Image indicator, Color color, bool status) {
        Color chosenColor = color;
        if (status) {
            chosenColor = color * 2; //can be problem with alfa
        }
        indicator.color = chosenColor;
    }

    private bool CheckSync(ForgeMButtonT type) {
        return playerPressedButton[(int)type, playerIndex] && playerPressedButton[(int)type, otherPlayerIndex];
    }

    private bool CheckSyncStatus(ForgeMButtonT type)
    {
        if (timeOfPress == -1)
        {
            timeOfPress = GetTime();
            return false;
        }
        else if (timeOfPress + buttonTime >= GetTime())
        {
            if (CheckSync(type))
            {
                //Good
                ResetTimers();
                ResetButtonStates();
                return true;
            }
        }
        //Not good
        ResetButtonStates();
        return false;
    }

    protected override void OnMinigameOpened()
    {
        base.OnMinigameOpened();
        ResetFails();
        playerIndex = connectedWorkstation.subscribedUsersIDs.IndexOf(AUtils._playerID);
        otherPlayerIndex = 1 - playerIndex;
    }

    protected override void OnMinigameClosed()
    {
        base.OnMinigameClosed();
        
        ResetButtonStates();
    }

    private void ResetButtonStates()
    {
        for (int i = 0; i < playerPressedButton.GetLength(0); i++)
        {
            for (int j = 0; j < playerPressedButton.GetLength(1); j++)
            {
                playerPressedButton[i, j] = false;
            }
        }
        
    }

}
