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
    public RectTransform pointOnBar;
    public float barWidth;

    public List<ButtonDistancePair> buttonPairs;

    private static readonly float SUCCESS_RANGE = 1f;

    protected override void Awake()
    {
        corespondingWorkstationType = WorkstationType.Forge;
        base.Awake();
    }

    protected override void Start()
    {

        base.Start();

        
        foreach (var pair in buttonPairs)
        {
            pair.button.onClick.AddListener(() => MovePoint(pair.moveDistance));
        }
    }

    public void SetTargetPosition(float target)
    {
        targetProgressCout = Mathf.Clamp(target, -barWidth / 2, barWidth / 2);
    }

    private void MovePoint(float distance)
    {
        progressCount = Mathf.Clamp(progressCount + distance, -barWidth / 2, barWidth / 2);
        UpdatePointPosition();
        CheckIfTargetReached();
    }

    private void UpdatePointPosition()
    {
        //TODO change position
    }

    private void CheckIfTargetReached()
    {
        if (Mathf.Abs(progressCount - targetProgressCout) <= SUCCESS_RANGE)
        {
            Success();
        }
    }

    protected override void OnMinigameOpened()
    {
        progressCount = 0;
        UpdatePointPosition();
    }

    public override void Success()
    {

        base.Success();
    }

    protected override void HandleInput()
    {
        
    }
}
