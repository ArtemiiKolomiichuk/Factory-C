using UnityEngine;
using UnityEngine.UI;

public class SlimeKneadingMinigame : MinigameInterface
{
    [Header("Slime Kneading Minigame Settings")]

    private static readonly float HOLD_DURATION = 2f;
    private float currentHoldTime = 0f;

    private bool isHolding = false;
    private bool canProgress = true;

    private const float PROGRESS_DELTA = 1;
    
    public Button buttonToHold = null;
    public TimeCounterBarController timerBar = null;

    private const KeyCode BUTTON_TO_PRESS = KeyCode.J;

    protected override void Awake()
    {
        corespondingWorkstationType = WorkstationType.Slime;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void StartKneading()
    {
        if (!isHolding && canProgress)
        {
            isHolding = true;
            currentHoldTime = 0f;
            if (timerBar != null) {
                timerBar.StartTimer();
            }
            Debug.Log("Started kneading the slug! Hold for 2 seconds");
        }
    }

    protected override void Update()
    {
        base.Update();
        
    }

    private void CheckHolding() {
        if (isHolding)
        {
            currentHoldTime += Time.deltaTime;
        }
    }

    public void OnButtonRelease()
    {
        Debug.Log("Was holding button for:"+currentHoldTime);
        if (!isHolding)
        {
            return;
        }
        if (currentHoldTime >= HOLD_DURATION)
        {
            AddProgress();
        }
        if (timerBar != null) {
            timerBar.ResetBar();
        }
        isHolding = false;
        currentHoldTime = 0f;
    }

    private void AddProgress()
    {
        ChangeProgressCount(PROGRESS_DELTA);
        Debug.Log($"Progress step: {progressCount} ");

        if (progressCount >= targetProgressCount)
        {
            Success();
        }
    }

    public override void Success()
    {
        base.Success();
    }

    protected override void OnMinigameOpened()
    {
        base.OnMinigameOpened();
        //progressCount = 0; //TODO think about it
        //SetProgressBar(0);
    }

    protected override void HandleInput()
    {
        CheckHolding();
        if (Input.GetKeyDown(BUTTON_TO_PRESS)) //maybe change later
        {
            StartKneading();
        }
        if (Input.GetKeyUp(BUTTON_TO_PRESS))
        {
            OnButtonRelease();
        }
    }
}
