using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using Assets.Scripts.ItemsScripts;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
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
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    void Start()
    {
        Behavior = BehaviorType.Roaming;
        StartCoroutine(Wander());
    }


    IEnumerator Wander()
    {
        while (true)
        {
            Vector3 destination = RandomNavmeshLocation(10f);
            agent.SetDestination(destination);

            while (Vector3.Distance(transform.position, destination) > 1f)
            {
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
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
        
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

    public void Attack(Adventurer adventurer)
    {
        Debug.Log($"{name} attacks {adventurer.name}.");
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
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
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
        if (lootPrefab != null )
        {
            GameObject loot = Instantiate(lootPrefab, transform.position, transform.rotation);
           
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }


    public void Attack() { Debug.Log("Monster attacks adventurer."); }
}
