using UnityEngine;

public interface IForestFactory
{
    Monster CreateMonster(Vector3 spawnPosition);
    void AssignResources(Monster monster);
}
