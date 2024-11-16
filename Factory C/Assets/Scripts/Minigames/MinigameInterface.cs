using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MinigameInterface : MonoBehaviour
{
    public GameObject minigameOverlay;
    public Button closeButton;
    public WorkstationType corespondingWorkstationType;

    protected float progressCount = 0;
    protected float targetProgressCout;

    protected Recipe recipeData;

    private Workstation connectedWorkstation = null;
    private ProgressBarScaler connectedWorkstationProgressBar = null;
    private MinigameAnimationController animController; // will be implemented when animation

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
        if (minigameOverlay != null) { 
            minigameOverlay.SetActive(state);
        }
    }

    protected void ChangeProgressCount(float delta) {
        progressCount += delta;
        if (connectedWorkstationProgressBar != null) {
            connectedWorkstationProgressBar.ChangeProgressBar(progressCount, targetProgressCout);
        }
    }

    private void SetConnectedWorkstation(Workstation newWorkstation) 
    {
        connectedWorkstation = newWorkstation;
        connectedWorkstationProgressBar = connectedWorkstation.progressBar;
        animController = connectedWorkstation.animController;
        if (connectedWorkstationProgressBar != null && connectedWorkstationProgressBar.getLastProgress() != 0) {
            connectedWorkstationProgressBar.getLastProgress();
        }
    }

    private void SetRecipe(Recipe newRecipe)
    {
        recipeData = newRecipe;
        targetProgressCout = recipeData.difficultyMod;
    }

    protected virtual void Start()
    {
        Activate(false);

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
        SetConnectedWorkstation(workstation);
        SetRecipe(recipe);
        
        ChangeProgressCount(0);
        Activate(true);
        
        OnMinigameOpened();
    }

    public virtual void Success()
    {
        Debug.Log("Success! "+ corespondingWorkstationType.ToString());
        ChangeProgressCount(-progressCount);
        CloseMinigame();
        if (connectedWorkstation != null) { 
            connectedWorkstation.SucceedProcessing(recipeData);
        }
    }

    public void CloseMinigame()
    {
        Activate(false);
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
