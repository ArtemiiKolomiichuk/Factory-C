using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : NetworkBehaviour
{
    [SerializeField]
    private float walkingRange;
    [SerializeField]
    private float waitingTime;
    private float currentWaitTime = 0;
    private bool isWaiting = false;
    private NavMeshAgent agent;
    private Vector3 destinationPoint;
    private CustomerState state = CustomerState.WAITING;
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!IsHost)
            return;*/
        currentWaitTime += Time.deltaTime;
        if(state == CustomerState.WAITING) {
            WaitForOrderCompletion();
        }
        else if(state == CustomerState.GO_HOME && ((HasStoped() && currentWaitTime > 1) || currentWaitTime > 20)) {
            Debug.Log(currentWaitTime);
            Destroy(gameObject);
        }
    }

    public void SetState(CustomerState state) {
        this.state = state;
        if(this.state == CustomerState.INTERACT_WITH_PLAYER) {
            agent.SetDestination(gameObject.transform.position);
        }
        else if(this.state == CustomerState.GO_HOME) {
            currentWaitTime = 0;
            agent.SetDestination(spawnPoint);
        }
    }

    private void WaitForOrderCompletion() {
        if(isWaiting) {
            Waiting();
        }
        else {
            Walking();
        }
    }

    private void Waiting() {
            
            //Debug.Log(currentWaitTime);
            //Debug.Log(waitTime);
            if(currentWaitTime > waitingTime) {
                currentWaitTime = 0;
                isWaiting = false;
            } 
    }

    private void Walking() {
        if(destinationPoint == null) {
            destinationPoint = RandomNavmeshLocation(walkingRange);
        } 
        agent.SetDestination(destinationPoint);
        //Debug.Log(destinationPoint);
        //Debug.Log(transform.localPosition);
        if(HasStoped() || currentWaitTime > waitingTime) {
            destinationPoint = RandomNavmeshLocation(walkingRange);
            currentWaitTime = 0;
            isWaiting = true;
        }
    }

    private bool HasStoped() {
        float dist=agent.remainingDistance;
        return 
            dist != Mathf.Infinity 
            && agent.pathStatus == NavMeshPathStatus.PathComplete 
            && dist == 0;
    }

    private Vector3 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.localPosition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
