using UnityEngine;

public interface IForestNPCController
{
    void SetBehavior(Monster monster, BehaviorType behaviorType);
    void Move(Monster monster, Vector3 targetPosition);
    void Engage(Monster monster, GameObject target);
}
