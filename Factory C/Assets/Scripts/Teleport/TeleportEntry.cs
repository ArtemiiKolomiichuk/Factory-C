using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEntry : MonoBehaviour
{
    
    [SerializeField]
    private Teleport teleport;
    [SerializeField]
    private string teleportMessage;

    private GameObject currentPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && currentPlayer != null) {
            teleport.DoneTeleport(this.gameObject.transform, currentPlayer, teleportMessage);
            currentPlayer = null;
        }
    }

    public void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        currentPlayer = other.gameObject;
    }

    public void OnTriggerExit(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        currentPlayer = null;
    }
}
