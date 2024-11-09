using UnityEngine;

public class ForestNPCController : MonoBehaviour, IForestNPCController
{
    public void SetBehavior(Monster monster, BehaviorType behaviorType)
    {
        // Debug message for setting the monster's behavior
        Debug.Log($"Setting monster's behavior to {behaviorType}.");
    }

    public void Move(Monster monster, Vector3 targetPosition)
    {
        // Debug message for moving the monster
        Debug.Log("Moving monster to target position.");
    }

    public void Engage(Monster monster, GameObject target)
    {
        // Debug message for monster engaging with a target
        Debug.Log("Monster is engaging with a target.");
    }
}
