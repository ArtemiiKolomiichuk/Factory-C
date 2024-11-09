using UnityEngine;

public class ForestNPCController : MonoBehaviour, IForestNPCController
{
    public void SetBehavior(Monster monster, BehaviorType behaviorType)
    {
        monster.Behavior = behaviorType;
        Debug.Log($"Behavior set to {behaviorType} for monster.");
    }

    public void Move(Monster monster, Vector3 targetPosition)
    {
        monster.transform.position = Vector3.MoveTowards(monster.transform.position, targetPosition, monster.Speed * Time.deltaTime);
    }

    public void Engage(Monster monster, GameObject target)
    {
        if (target.TryGetComponent(out Adventurer adventurer))
        {
            monster.Attack(adventurer);
            Debug.Log("Monster is engaging an adventurer.");
        }
    }
}
