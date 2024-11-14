using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField]
    private float walkRange;
    [SerializeField]
    private float waitTime;
    private float currentWaitTime = 0;
    private bool isWaitingForOrderCompletion = true;
    private bool isWaiting = false;
    private NavMeshAgent agent;
    private Vector3 destinationPoint;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isWaitingForOrderCompletion) {
            WaitForOrderCompletion();
        }
        else if(HasStoped()) {
            Destroy(gameObject);
        }
        //Debug.Log(1);
    }

    public void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        outline.enabled = true;
    }

    public void OnTriggerExit(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        outline.enabled = false;
    }

    public void GoToPointThenDestroy(Vector3 point) {
        isWaitingForOrderCompletion = false;
        agent.SetDestination(point);
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
            currentWaitTime += Time.deltaTime;
            //Debug.Log(currentWaitTime);
            //Debug.Log(waitTime);
            if(currentWaitTime > waitTime) {
                currentWaitTime = 0;
                isWaiting = false;
            } 
    }

    private void Walking() {
        if(destinationPoint == null) {
            destinationPoint = RandomNavmeshLocation(walkRange);
        } 
        agent.SetDestination(destinationPoint);
        //Debug.Log(destinationPoint);
        //Debug.Log(transform.localPosition);
        if(HasStoped()) {
            destinationPoint = RandomNavmeshLocation(walkRange);
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
