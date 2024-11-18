using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonDistancePair
{
    public Button button;
    public float moveDistance;
    

    public ButtonDistancePair(Button button, float moveDistance)
    {
        this.button = button;
        this.moveDistance = moveDistance;
    }
}

public class SmithingMinigame : MinigameInterface
{
    [Header("Smithing Minigame Settings")]
    public RectTransform redPoint;
    public RectTransform bluePoint;
    public RectTransform bar;
    public float maxProgress = 1500;
    private float barWidth;
    private float barDelta;
    private float barPoint;

    public List<ButtonDistancePair> buttonPairs;

    private static readonly float SUCCESS_RANGE = 0.1f;

    protected override void Awake()
    {
        corespondingWorkstationType = WorkstationType.Anvil;
        base.Awake();
    }

    protected override void Start()
    {
        barWidth = bar.rect.width;
        barDelta = -barWidth / 2;
        barPoint = maxProgress / barWidth;

        base.Start();

        foreach (var pair in buttonPairs)
        {
            pair.button.onClick.AddListener(() => MovePoint(pair.moveDistance));
        }
        
    }

    public void SetTargetPosition(float target)
    {
        float convertedProgress = target * barPoint;
        redPoint.anchoredPosition = new Vector2(convertedProgress + barDelta, 
                                                redPoint.anchoredPosition.y);
    }

    public void MovePoint(float distance)
    {
        progressCount = progressCount + distance;
        UpdatePointPosition();
        CheckIfTargetReached();
    }

    private void UpdatePointPosition()
    {
        float convertedProgress = progressCount * barPoint;
        bluePoint.anchoredPosition = new Vector2(convertedProgress + barDelta,
                                                 bluePoint.anchoredPosition.y);

        float proximity = AUtils.CalculateProximity(progressCount, targetProgressCount, 0, maxProgress);
        SetProgressCount(proximity, 1);
        Debug.Log("WTF | UpdatetPointPosition: "+ progressCount +" pix:" + bluePoint.anchoredPosition + "proximity:"+proximity);
    }

    private void CheckIfTargetReached()
    {
        if (progressCount < 0 || progressCount > maxProgress)
        {
            Fail();
            return;
        }
        if (Mathf.Abs(progressCount - targetProgressCount) <= SUCCESS_RANGE)
        {
            Success();
            return;
        }
    }

    protected override void OnMinigameOpened()
    {
        Debug.Log("WTF | SmithingMinigameOpened");
        SetTargetPosition(targetProgressCount);
        UpdatePointPosition();
    }

    protected override void OnMinigameClosed() 
    {
        SetProgressCount(0, 1);
    }

    public override void Success()
    {
        base.Success();
    }

    protected override void HandleInput()
    {
        
    }
}
