using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AdventurerSpawner : MonoBehaviour
{
    public GameObject adventurerPrefab;
    public int minGroupSize = 2;
    public int maxGroupSize = 3;
    public int maxAdventurers = 10;
    public float spawnRadius = 100f;
    public float spawnInterval = 10f;

    void Start()
    {
        if(NetworkManager.Singleton.IsServer)
            StartCoroutine(SpawnAdventurerGroups());
    }

    IEnumerator SpawnAdventurerGroups()
    {
        while (true)
        {
            if(Adventurer.AdventurerCount < maxAdventurers)
            {
                SpawnGroup();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGroup()
    {
        int groupSize = Random.Range(minGroupSize, maxGroupSize + 1);
        List<Adventurer> groupMembers = new List<Adventurer>();
        Vector3 groupPosition = RandomNavmeshLocation(spawnRadius);

        for (int i = 0; i < groupSize; i++)
        {
            Vector3 spawnPosition = groupPosition + Random.insideUnitSphere * 2f;
            GameObject adventurerGO = Instantiate(adventurerPrefab, spawnPosition, Quaternion.identity);
            Adventurer adventurer = adventurerGO.GetComponent<Adventurer>();
            groupMembers.Add(adventurer);
        }

        foreach (var member in groupMembers)
        {
            member.SetGroupMembers(groupMembers);
        }
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}
