using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MinigameInterface : MonoBehaviour
{
    public GameObject minigameOverlay;
    public Button closeButton;
    public string minigameName;

    protected float progressCount = 0;
    protected float targetProgressCout;

    public Workstation connectedWorkstation = null;

    private static Dictionary<string, MinigameInterface> minigames = new Dictionary<string, MinigameInterface>();

    public void Activate(bool state) {
        enabled = state;
    }

    protected virtual void Start()
    {
        minigameOverlay.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseMinigame);
        }

        if (!string.IsNullOrEmpty(minigameName))
        {
            if (!minigames.ContainsKey(minigameName))
            {
                minigames[minigameName] = this;
            }
            else
            {
                Debug.LogWarning($"Minigame with name '{minigameName}' already exists.");
            }
        }
        else
        {
            Debug.LogWarning("Minigame name is not set!");
        }
    }

    public static MinigameInterface GetMinigameByName(string name)
    {
        minigames.TryGetValue(name, out MinigameInterface minigame);
        return minigame;
    }

    public void OpenMinigame(Workstation workstation, float targetProgress)
    {
        connectedWorkstation = workstation;
        targetProgressCout = targetProgress;
        Activate(true);
        minigameOverlay.SetActive(true);
        OnMinigameOpened();
    }

    public virtual void Success()
    {
        Debug.Log("Success! "+minigameName);
        CloseMinigame();
        if (connectedWorkstation != null) { 
            connectedWorkstation.SucceedProcessing();
        }
    }

    public void CloseMinigame()
    {
        Activate(false);
        minigameOverlay.SetActive(false);
        OnMinigameClosed();
    }

    protected virtual void Update()
    {   
        HandleInput();
    }

    protected abstract void HandleInput();

    protected virtual void OnMinigameOpened() { }
    protected virtual void OnMinigameClosed() { }
}
