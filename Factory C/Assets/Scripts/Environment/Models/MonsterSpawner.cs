using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;


public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs;
    public int maxMonsters = 10;
    public int spawnCount = 1;
    public float spawnRadius = 30f;
    public float spawnInterval = 2f;

    void Start()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        yield return new WaitForSeconds(6);
        while (true) { 
            if (Monster.MonsterCount < maxMonsters)
            {
                Vector3 spawnPosition = RandomNavmeshLocation(spawnRadius);

                int randomIndex = Random.Range(0, monsterPrefabs.Count);
                GameObject selectedPrefab = monsterPrefabs[randomIndex];

                var m = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                m.GetComponent<NetworkObject>().Spawn();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
       
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}