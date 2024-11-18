using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; 
    public int spawnCount = 2;
    public float spawnRadius = 30f;

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = RandomNavmeshLocation(spawnRadius);

            int randomIndex = Random.Range(0, monsterPrefabs.Count);
            GameObject selectedPrefab = monsterPrefabs[randomIndex];

            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}