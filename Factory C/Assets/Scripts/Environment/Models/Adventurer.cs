using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using Assets.Scripts.ItemsScripts;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Adventurer : MonoBehaviour
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
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
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
                    monster.TakeDamage(10); 
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
            GameObject loot = Instantiate(lootPrefab, transform.position, transform.rotation);

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
        Destroy(gameObject);
    }

  
    public void StartRaid() { Debug.Log("Adventurer starts raid."); }
    public void EndRaid() { Debug.Log("Adventurer ends raid."); }
}