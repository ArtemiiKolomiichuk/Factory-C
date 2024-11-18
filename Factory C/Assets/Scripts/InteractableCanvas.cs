using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableCanvas : MonoBehaviour
{

    public static InteractableCanvas Instance { get; private set; }

    [SerializeField]
    private string defaultHint = "Press [E] to interact";
    [SerializeField]
    private TextMeshProUGUI hintText;

    private uint hintCounter = 0;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHint(string hint = null) {
        hintCounter++;
        hintText.text = hint ?? defaultHint;
        gameObject.SetActive(true);
    }

    public void HideHint() {
        hintCounter = hintCounter == 0 ? 0 : hintCounter-1;
        //Debug.Log(hintCounter);
        if(hintCounter == 0) {
            gameObject.SetActive(false);
        }
    }
}
