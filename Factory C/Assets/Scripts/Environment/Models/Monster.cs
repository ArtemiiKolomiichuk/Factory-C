using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.Netcode;

public class Monster : NetworkBehaviour
{
    public static int MonsterCount = 0;
    public float Speed { get; set; } = 6.5f;
    public BehaviorType Behavior { get; set; }

    [SerializeField]
    public int Health { get; set; } = 100;
    public GameObject lootPrefab; 
    public bool IsAggressive { get; set; } = false;
    public List<Interactable> Items { get; set; }

    private NavMeshAgent agent;

    void Awake()
    {
        MonsterCount++;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    void Start()
    {
        Behavior = BehaviorType.Roaming;
        if (!NetworkManager.Singleton.IsServer) return;
        StartCoroutine(Wander());
    }


    IEnumerator Wander()
    {
        while (true)
        {
            Vector3 destination = RandomNavmeshLocation(12f);
            agent.SetDestination(destination);

            while (Vector3.Distance(transform.position, destination) > 1f)
            {
                PlayerMovement3D player = FindNearestPlayer();
                if (player != null)
                {
 
                    Follow(player);
                    
                    //yield return new WaitForSeconds(2f);
                    break;
                }
                Adventurer adventurer = FindNearestAdventurer();
                if (adventurer != null)
                {
                    Attack(adventurer);
                    yield return new WaitForSeconds(2f);
                }
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }


    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

 
    Adventurer FindNearestAdventurer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 7f);
        
        foreach (var hit in hits)
        {
            Adventurer adventurer = hit.GetComponent<Adventurer>();
            
            if (adventurer != null)
            {
                
                return adventurer;
            }
        }
        return null;
    }

    PlayerMovement3D FindNearestPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 15f);

        foreach (var hit in hits)
        {
            PlayerMovement3D player = hit.GetComponent<PlayerMovement3D>();
           // print(player);

            if (player != null&&!player.isDisguised)
            {

                return player;
            }
        }
        return null;
    }

    public void Attack(Adventurer adventurer)
    {
        agent.SetDestination(adventurer.transform.position);
        StartCoroutine(AttackCoroutine(adventurer));
    }
    IEnumerator AttackCoroutine(Adventurer adventurer)
    {
        while (adventurer != null && adventurer.Health > 0)
        {
            if (Vector3.Distance(transform.position, adventurer.transform.position) <= 4f)
            {
                float chance = Random.Range(0f, 1f);
                if (chance <= 0.2f) 
                {
                    adventurer.TakeDamage(100);
                }
                else
                {
                    adventurer.TakeDamage(20); 
                }
            }

            if (adventurer.Health <= 0)
            {
                yield return Wander();
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void Follow(PlayerMovement3D player)
    { 
        agent.SetDestination(new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        MonsterCount--;
        if (lootPrefab != null )
        {
            Vector3 positionPrefab = new Vector3(transform.position.x, transform.position.y+5, transform.position.z);
            GameObject loot = Instantiate(lootPrefab, positionPrefab, transform.rotation);
            loot.GetComponent<NetworkObject>().Spawn();
        }
        StopAllCoroutines();
        GetComponent<NetworkObject>().Despawn(true);
    }
}
