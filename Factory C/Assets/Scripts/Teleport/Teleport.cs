using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleport : MonoBehaviour
{

    [SerializeField]
    private Transform teleportEntry1;
    [SerializeField]
    private Transform teleportEntry2;
    [SerializeField]
    private Transform teleportHub;
    [SerializeField]
    private float teleportTime;
    private float currentTeleportTime;
    private Transform toTeleportEntry;
    private bool isTeleport;
    private GameObject playerToTeleport;
    [SerializeField]
    private Canvas teleportCanvas;
    [SerializeField]
    private TextMeshProUGUI teleportMessage;
    [SerializeField]
    private TextMeshProUGUI teleportTimeMessage;

    // Start is called before the first frame update
    void Start()
    {
        teleportCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTeleport) {
            currentTeleportTime += Time.deltaTime;
            if(currentTeleportTime > teleportTime) {
                isTeleport = false;
                playerToTeleport.transform.position = new Vector3(toTeleportEntry.position.x, playerToTeleport.transform.position.y, toTeleportEntry.position.z);
                playerToTeleport.transform.rotation = toTeleportEntry.rotation;
                toTeleportEntry = null;
                teleportCanvas.enabled = false;
                this.playerToTeleport = null;
                return;
            }
            this.teleportTimeMessage.text = ((int)(teleportTime - currentTeleportTime + 1.0)).ToString();
            if(teleportTime - currentTeleportTime < 0.2f) {
                playerToTeleport.transform.position = new Vector3(toTeleportEntry.position.x, playerToTeleport.transform.position.y, toTeleportEntry.position.z);
                playerToTeleport.transform.rotation = toTeleportEntry.rotation;
            }
        }
    }

    public void DoneTeleport(Transform fromTeleportEntry, GameObject playerToTeleport, string teleportMessage) {
        if(fromTeleportEntry == teleportEntry1) {
            toTeleportEntry = teleportEntry2;
        }
        else if(fromTeleportEntry == teleportEntry2) {
            toTeleportEntry = teleportEntry1;
        }
        if(toTeleportEntry == null) {
            return;
        }
        currentTeleportTime = 0.001f;
        this.playerToTeleport = playerToTeleport;
        this.teleportMessage.text = teleportMessage;
        this.teleportTimeMessage.text = ((int)(teleportTime - currentTeleportTime + 1.0)).ToString();
        this.teleportCanvas.enabled = true;
        this.isTeleport = true;
        playerToTeleport.transform.position = new Vector3(teleportHub.position.x, playerToTeleport.transform.position.y, teleportHub.position.z);
        playerToTeleport.transform.rotation = teleportHub.rotation;
    }
}
