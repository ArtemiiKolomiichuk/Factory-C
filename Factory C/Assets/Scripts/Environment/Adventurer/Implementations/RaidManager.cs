using System.Collections.Generic;
using UnityEngine;

public class RaidManager : MonoBehaviour, IRaidManager
{
    public List<Adventurer> ActiveAdventurers { get; private set; } = new List<Adventurer>();

    public void StartRaid()
    {
        Debug.Log("Raid started!");
        foreach (var adventurer in ActiveAdventurers)
        {
            adventurer.StartRaid();
        }
    }

    public void EndRaid()
    {
        Debug.Log("Raid ended.");
        foreach (var adventurer in ActiveAdventurers)
        {
            adventurer.EndRaid();
        }
    }

    public void AssignAdventurers(List<Adventurer> adventurers)
    {
        ActiveAdventurers = adventurers;
        Debug.Log($"{adventurers.Count} adventurers assigned to raid.");
    }
}
