using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOutlineUI : MonoBehaviour
{
    [SerializeField, Multiline]
    private string hint;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        if(outline != null) outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        //Debug.Log($"{IsLocalPlayer} {IsHost}");
        //if(!IsOwner) return;
        if(!other.CompareTag("Player")) {
            return;
        }
        if(outline != null) outline.enabled = true;
        InteractableCanvas.Instance.ShowHint(hint);
    }

    public void OnTriggerExit(Collider other) {
        //if(!IsOwner) return;
        if(!other.CompareTag("Player")) {
            return;
        }
        if(outline != null) outline.enabled = false;
        InteractableCanvas.Instance.HideHint();
    }
}
