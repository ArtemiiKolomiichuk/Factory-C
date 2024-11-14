using UnityEngine;

public class ForestFactory : MonoBehaviour, IForestFactory
{
    public Monster CreateMonster(Vector3 spawnPosition)
    {
        // Debug message for creating a new monster
        Debug.Log("Creating a new monster at spawn position.");
        return null; // Placeholder return
    }

    public void AssignResources(Monster monster)
    {
        // Debug message for assigning resources to a monster
        Debug.Log("Assigning resources to monster.");
    }
}
