using UnityEngine;

public interface IAdventurerFactory
{
    Adventurer CreateAdventurer(Vector3 spawnPosition);
    void AssignLoot(Adventurer adventurer);
}
