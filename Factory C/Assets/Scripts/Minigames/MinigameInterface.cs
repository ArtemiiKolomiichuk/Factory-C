using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

public abstract class MinigameInterface : MonoBehaviour
{
    public GameObject minigameOverlay;
    public Button closeButton;
    public WorkstationType corespondingWorkstationType;

    protected float progressCount = 0;
    protected float targetProgressCout;

    public Workstation connectedWorkstation = null;

    private static Dictionary<WorkstationType, MinigameInterface> MINI_GAMES = new Dictionary<WorkstationType, MinigameInterface>();

    protected virtual void Awake() {
        if (!MINI_GAMES.ContainsKey(corespondingWorkstationType))
        {
            MINI_GAMES[corespondingWorkstationType] = this;
        }
        else
        {
            Debug.LogWarning($"Minigame with name '{corespondingWorkstationType.ToString()}' already exists.");
        }
    }

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
 
    }

    public static MinigameInterface GetMinigameByType(WorkstationType type)
    {
        MINI_GAMES.TryGetValue(type, out MinigameInterface minigame);
        return minigame;
    }

    public void OpenMinigame(Workstation workstation, Recipe recipe)
    {
        connectedWorkstation = workstation;
        targetProgressCout = recipe.difficultyMod;
        Activate(true);
        minigameOverlay.SetActive(true);
        OnMinigameOpened();
    }

    public virtual void Success()
    {
        Debug.Log("Success! "+ corespondingWorkstationType.ToString());
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
