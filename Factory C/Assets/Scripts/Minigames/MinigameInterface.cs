using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MinigameInterface : MonoBehaviour
{
    public GameObject minigameOverlay;
    public Button closeButton;
    protected WorkstationType corespondingWorkstationType;

    protected float progressCount = 0;
    protected float targetProgressCount;

    protected Recipe recipeData;

    protected Workstation connectedWorkstation = null;
    protected ProgressBarScaler connectedWorkstationProgressBar = null;
    protected MinigameAnimationController animController; // will be implemented when animation

    public ProgressBarScaler uiProgressBar = null;

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
        if (!state)
        {
            Debug.Log("Deactivated Minigame Interface");
        }
        else { 
            Debug.Log("Activated Minigame Interface");
        }

        enabled = state;
        if (minigameOverlay != null) { 
            minigameOverlay.SetActive(state);
        }
    }

    protected void ChangeProgressCount(float delta) {
        progressCount += delta;
        if (connectedWorkstationProgressBar != null) {
            connectedWorkstationProgressBar.ChangeProgressBar(progressCount, targetProgressCount);
        }
        if (uiProgressBar != null) {
            uiProgressBar.ChangeProgressBar(progressCount, targetProgressCount);
        }
    }

    protected void SetProgressCount(float count, float target)
    {
        if (connectedWorkstationProgressBar != null)
        {
            connectedWorkstationProgressBar.ChangeProgressBar(count, target);
        }
        if (uiProgressBar != null)
        {
            uiProgressBar.ChangeProgressBar(count, target);
        }
    }

    private void SetConnectedWorkstation(Workstation newWorkstation) 
    {
        connectedWorkstation = newWorkstation;
        connectedWorkstationProgressBar = connectedWorkstation.progressBar;
        animController = connectedWorkstation.animController;
        progressCount = 0;
        if (connectedWorkstationProgressBar != null && connectedWorkstationProgressBar.getLastProgress() > 0) {
            progressCount = connectedWorkstationProgressBar.getLastProgress();
        }
    }

    private void SetRecipe(Recipe newRecipe)
    {
        recipeData = newRecipe;
        targetProgressCount = recipeData.difficultyMod;
    }

    protected virtual void Start()
    {
        Debug.Log("Start Deactivation VVV");
        Activate(false);
        Debug.Log("Start Deactivation AAA");

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
        
        Activate(true);
        ChangeProgressCount(0);
        
        OnMinigameOpened();
    }

    public virtual void Success()
    {
        Debug.Log("Success! "+ corespondingWorkstationType.ToString());
        ChangeProgressCount(-progressCount);
        
        if (connectedWorkstation != null) { 
            connectedWorkstation.SucceedProcessing(recipeData);
        }
        CloseMinigame();
    }

    public virtual void Fail()
    {
        Debug.Log("Fail! " + corespondingWorkstationType.ToString());
        ChangeProgressCount(-progressCount);
        
        if (connectedWorkstation != null)
        {
            connectedWorkstation.FailProcessing();
        }
        CloseMinigame();
    }

    public void CloseMinigame()
    {
        OnMinigameClosed();
        connectedWorkstation.UnsubscribeAllUser();
        Activate(false);
    }

    protected virtual void Update()
    {   
        HandleInput();
    }

    protected virtual void HandleInput() { }

    protected virtual void OnMinigameOpened() { }
    protected virtual void OnMinigameClosed() { }
}
