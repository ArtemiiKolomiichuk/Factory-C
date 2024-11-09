using System.Collections.Generic;
using UnityEngine;

public interface IRaidManager
{
    List<Adventurer> ActiveAdventurers { get; }
    void StartRaid();
    void EndRaid();
    void AssignAdventurers(List<Adventurer> adventurers);
}