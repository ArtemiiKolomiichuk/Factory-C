using System.Collections.Generic;
using UnityEngine;

public interface IForestManager
{
    List<Monster> Monsters { get; }
    void UpdateForest();
    void SpawnTraps();
    void ResetAggression();
}
