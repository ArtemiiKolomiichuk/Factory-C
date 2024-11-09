using System.Collections.Generic;
using UnityEngine;

public class ForestManager : MonoBehaviour, IForestManager
{
    public List<Monster> Monsters { get; private set; } = new List<Monster>();

    public void UpdateForest()
    {
        Debug.Log("Forest state updated.");
    }

    public void SpawnTraps()
    {
        Debug.Log("Traps generated in the forest.");
    }

    public void ResetAggression()
    {
        foreach (var monster in Monsters)
        {
            monster.IsAggressive = false;
        }
        Debug.Log("Aggression reset in the forest.");
    }
}
