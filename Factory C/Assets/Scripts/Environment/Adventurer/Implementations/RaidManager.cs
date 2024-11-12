using System.Collections.Generic;
using UnityEngine;

public class RaidManager : MonoBehaviour, IRaidManager
{
    public List<Adventurer> ActiveAdventurers { get; private set; } = new List<Adventurer>();

    public void StartRaid()
    {
        // Debug message for starting the raid
        Debug.Log("Starting raid - adventurers begin their journey to the forest.");
    }

    public void EndRaid()
    {
        // Debug message for ending the raid
        Debug.Log("Ending raid - adventurers return to base.");
    }

    public void AssignAdventurers(List<Adventurer> adventurers)
    {
        // Debug message for assigning adventurers to the raid
        ActiveAdventurers = adventurers;
        Debug.Log($"Assigned {adventurers.Count} adventurers to the raid.");
    }
}
