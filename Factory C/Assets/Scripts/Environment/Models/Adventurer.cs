using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Netcode;

public class Adventurer : NetworkBehaviour
{
    public static int AdventurerCount = 0;
    public float Speed { get; set; } = 2.0f;
    public BehaviorType Behavior { get; set; }
    public List<Interactable> Items { get; set; }
    public int Health { get; set; } = 100;
    public GameObject lootPrefab;
    public bool isLeader = false;

    private NavMeshAgent agent;
    public List<Adventurer> groupMembers;

    void Awake()
    {
        AdventurerCount++;
        agent = GetComponent<NavMeshAgent>();
        //agent.speed = Speed;
        //groupMembers = new List<Adventurer>();
    }

    void Start()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        Behavior = BehaviorType.Roaming;
        if (isLeader)
        {
            StartCoroutine(Wander());
        }
        else
        {
            StartCoroutine(FollowLeader());
        }
    }


    IEnumerator FollowLeader()
    {
        while (true)
        {
            if (groupMembers.Count > 0)
            {
                Adventurer leader = groupMembers[0];
                if (leader != null)
                {
                    Vector3 followPosition = leader.transform.position - leader.transform.forward * 4f;
                    agent.SetDestination(followPosition);
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }


    IEnumerator Wander()
    {
        while (true)
        {
            Vector3 destination = RandomNavmeshLocation(10f);
            agent.SetDestination(destination);

            while (Vector3.Distance(transform.position, destination) > 1f)
            {

                PlayerMovement3D player = FindNearestPlayer();
                if (player != null)
                {

                    Follow(player);

                    //yield return new WaitForSeconds(1f);
                    break;
                }
                Monster monster = FindNearestMonster();
                if (monster != null)
                {
                    foreach (var member in groupMembers)
                    {
                        member.Attack(monster);
                    }
                    yield return new WaitForSeconds(3f);
                }
                yield return null;
            }
            yield return new WaitForSeconds(3f);
        }
    }


    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1);
        return hit.position;
    }


    Monster FindNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 7f);
        foreach (var hit in hits)
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                return monster;
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

            if (player != null && player.isDisguised)
            {

                return player;
            }
        }
        return null;
    }
    public void Follow(PlayerMovement3D player)
    {
        print("Following A");
        //agent.Se
        //while(player.isDisguised)
        agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

    }

    public void Attack(Monster monster)
    {
        Debug.Log($"{name} {Health} attacks {monster.name} {monster.Health}.");
        agent.SetDestination(monster.transform.position);
        StartCoroutine(AttackCoroutine(monster));

    }

    IEnumerator AttackCoroutine(Monster monster)
    {
        while (monster != null && monster.Health > 0)
        {
            
            if (Vector3.Distance(transform.position, monster.transform.position) <= 4f)
            {
                float chance = Random.Range(0f, 1f);
                if (chance <= 0.2f) 
                {
                    //print("attack1");
                    monster.TakeDamage(100);
                }
                else
                {
                    //print("attack2");
                    monster.TakeDamage(20); 
                }
            }

            if (monster.Health <= 0)
            {
                yield return Wander();
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetGroupMembers(List<Adventurer> members)
    {
        groupMembers = members;
        isLeader = groupMembers[0] == this;
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
        AdventurerCount--;
        if (lootPrefab != null )
        {
            Vector3 positionPrefab = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            GameObject loot = Instantiate(lootPrefab, positionPrefab, transform.rotation);
            loot.GetComponent<NetworkObject>().Spawn();
        }
 
        if (groupMembers != null)
        {
            groupMembers.Remove(this);
        }

        if (isLeader && groupMembers.Count > 0)
        {
            foreach (Adventurer adventurer in groupMembers)
            {
                if (adventurer != null)
                {
                    adventurer.isLeader = true;
                    adventurer.StopCoroutine(groupMembers[0].FollowLeader());
                    adventurer.StartCoroutine(groupMembers[0].Wander());
                    break;
                }
            }
            
            
        }
        StopAllCoroutines();
        DestroyRpc(new(GetComponent<NetworkObject>()));
    }

    [Rpc(SendTo.Server)]
    private void DestroyRpc(NetworkObjectReference networkObjectReference)
    {
        if(networkObjectReference.TryGet(out NetworkObject gameObject))
        {
            gameObject.Despawn(true);
        }
    }

  
    public void StartRaid() { Debug.Log("Adventurer starts raid."); }
    public void EndRaid() { Debug.Log("Adventurer ends raid."); }
}