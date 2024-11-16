using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private float openRotation = 90f;
    [SerializeField]
    private GameObject door;

    private bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        OpenDoor();
    }

    public void OnTriggerExit(Collider other) {
        if(!other.CompareTag("Player")) {
            return;
        }
        CloseDoor();
    }

    public void OpenDoor() {
        if(isOpened) return;
        door.transform.localRotation = Quaternion.Euler(door.transform.localEulerAngles.x, door.transform.localEulerAngles.y + openRotation, door.transform.localEulerAngles.z);
        isOpened = true;
    }

    public void CloseDoor() {
        if(!isOpened) return;
        door.transform.localRotation = Quaternion.Euler(door.transform.localEulerAngles.x, door.transform.localEulerAngles.y - openRotation, door.transform.localEulerAngles.z);
        isOpened = false;
    }
}
