using UnityEngine;

public class AdventurerFactory : MonoBehaviour, IAdventurerFactory
{
    public Adventurer CreateAdventurer(Vector3 spawnPosition)
    {
        // Debug message for creating an adventurer
        Debug.Log("Creating a new adventurer at spawn position.");
        return null; // Placeholder return
    }

    public void AssignLoot(Adventurer adventurer)
    {
        // Debug message for assigning loot to the adventurer
        Debug.Log("Assigning loot to adventurer.");
    }
}
