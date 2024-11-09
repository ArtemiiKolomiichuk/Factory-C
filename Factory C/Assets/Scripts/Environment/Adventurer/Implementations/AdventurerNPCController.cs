using UnityEngine;

public class AdventurerNPCController : MonoBehaviour, IAdventurerNPCController
{
    public void SetBehavior(Adventurer adventurer, BehaviorType behaviorType)
    {
        adventurer.Behavior = behaviorType;
        Debug.Log($"Behavior set to {behaviorType} for adventurer.");
    }

    public void Move(Adventurer adventurer, Vector3 targetPosition)
    {
        adventurer.transform.position = Vector3.MoveTowards(adventurer.transform.position, targetPosition, adventurer.Speed * Time.deltaTime);
    }

    public void Interact(Adventurer adventurer, GameObject target)
    {
        if (target.TryGetComponent(out Monster monster))
        {
            adventurer.Attack(monster);
            Debug.Log("Adventurer is interacting with a monster.");
        }
    }
}
