using UnityEngine;
using UnityEngine.UI;

public class SlugKneadingMinigame : MinigameInterface
{
    [Header("Slug Kneading Minigame Settings")]
    public GameObject slug;
    public RectTransform progressBar;

    private static readonly float HOLD_DURATION = 2f;
    private float currentHoldTime = 0f;

    private bool isHolding = false;
    private bool canProgress = true;

    protected override void Start()
    {
        minigameName = "Slug Kneading";
        base.Start();
    }

    private void StartKneading()
    {
        if (!isHolding && canProgress)
        {
            isHolding = true;
            currentHoldTime = 0f;
            Debug.Log("Started kneading the slug! Hold for 2 seconds");
        }
    }

    protected override void Update()
    {
        base.Update();
        CheckHolding();
    }

    private void CheckHolding() {
        if (isHolding)
        {
            currentHoldTime += Time.deltaTime;
        }
    }

    public void OnButtonRelease()
    {
        if (!isHolding)
        {
            return;
        }
        if (currentHoldTime >= HOLD_DURATION)
        {
            AddProgress();
        }

        isHolding = false;
        currentHoldTime = 0f;

        SetProgressBar(0);
    }

    private void SetProgressBar(float progress) { 
        //TODO reset progress bar
    }

    private void AddProgress()
    {
        progressCount++;
        Debug.Log($"Progress step {progressCount} added!");

        SetProgressBar(progressCount);

        if (progressCount >= targetProgressCout)
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
        progressCount = 0;
        SetProgressBar(0);
    }

    protected override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartKneading();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnButtonRelease();
        }
    }
}
