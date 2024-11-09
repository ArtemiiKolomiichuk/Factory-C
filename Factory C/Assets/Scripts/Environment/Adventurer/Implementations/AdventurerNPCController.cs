using UnityEngine;

public class AdventurerNPCController : MonoBehaviour, IAdventurerNPCController
{
    public void SetBehavior(Adventurer adventurer, BehaviorType behaviorType)
    {
        // Debug message for setting the behavior of an adventurer
        Debug.Log($"Setting adventurer's behavior to {behaviorType}.");
    }

    public void Move(Adventurer adventurer, Vector3 targetPosition)
    {
        // Debug message for moving the adventurer
        Debug.Log("Moving adventurer to target position.");
    }

    public void Interact(Adventurer adventurer, GameObject target)
    {
        // Debug message for adventurer interaction with a target
        Debug.Log("Adventurer is interacting with a target.");
    }
}
