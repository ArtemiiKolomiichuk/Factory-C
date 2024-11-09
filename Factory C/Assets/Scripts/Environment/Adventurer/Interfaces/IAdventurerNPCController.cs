using UnityEngine;

public interface IAdventurerNPCController
{
    void SetBehavior(Adventurer adventurer, BehaviorType behaviorType);
    void Move(Adventurer adventurer, Vector3 targetPosition);
    void Interact(Adventurer adventurer, GameObject target);
}
