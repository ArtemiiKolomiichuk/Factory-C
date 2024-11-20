using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkstationType
{
    None,
    Anvil,
    Slime,
    Forge
}

public class Workstation : MonoBehaviour, UniverslaResourceHolderInterface, UsableInterface
{
    [Header("Workstation Configuration")]
    public WorkstationType workstationType;
    private List<Recipe> recipes;

    public List<ResourceType> inputResources;
    public List<ResourceType> storage; //for sorted input array

    private MinigameInterface corespondingMinigame;

    public List<int> subscribedUsersIDs = new List<int>();
    private int numberOfCurrentUsers = 0;
    public int numberOfMaxUsers = 0;

    public ProgressBarScaler progressBar;
    public MinigameAnimationController animController;

    private bool resourcesListChanged = false;
    private bool lockedResources = false;

    private void setResourceListChanged(bool state) {
        resourcesListChanged = state;
        if (progressBar != null && state) {
            progressBar.ChangeProgressBar(0, 1);
        }
    }

    public void Start()
    {
        corespondingMinigame = MinigameInterface.GetMinigameByType(workstationType);
        recipes = ResourceController.Instance.recipeDictionary[workstationType];
    }

    

    public bool SubscribeUser()
    {
        if (subscribedUsersIDs.Contains(AUtils._playerID)) {
            Debug.Log("WTF | User " + AUtils._playerID + " already subscribed to workstation:" + gameObject.ToString());
            return false;
        }
        
        if (numberOfCurrentUsers < numberOfMaxUsers)
        {
            subscribedUsersIDs.Add(AUtils._playerID);
            numberOfCurrentUsers++;
            return true;
        }
        Debug.Log("Workstation user list is full:" + gameObject.ToString());
        return false;
    }

    public bool UnsubscribeUser() {
        if (!subscribedUsersIDs.Contains(AUtils._playerID))
        {
            Debug.Log("WTF | User " + AUtils._playerID + " not subscribed to workstation:" + gameObject.ToString());
            return false;
        }

        if (numberOfCurrentUsers > 0)
        {
            subscribedUsersIDs.Remove(AUtils._playerID);
            numberOfCurrentUsers--;    
            return true;
        }

        Debug.Log("Workstation user list is empty:" + gameObject.ToString());
        return false;
    }

    public void UnsubscribeAllUser()
    {
        subscribedUsersIDs.Clear();
        numberOfCurrentUsers = 0;
    }

    public bool ValidNumberOfUsers()
    {
        return numberOfCurrentUsers == numberOfMaxUsers;
    }

    public Recipe CheckNeededResources() {
        foreach (Recipe recipe in recipes) {
            if (recipe.ThereOnlyNeededResources(storage)) {
                return recipe;
            }
        }

        return null;
    }

    public bool UseWorkstation() {
        Recipe toProcess;

        if (!subscribedUsersIDs.Contains(AUtils._playerID))
        {
            toProcess = CheckNeededResources();
            setResourceListChanged(false);

            if (toProcess == null)
            {
                Debug.Log("There no needed resources in workstation for any recipe");
                return false;
            }

            if (!SubscribeUser()) { 
                return false;
            }
        }
        else
        {
            return UnsubscribeUser();
        }

        return RunWorkstation(toProcess);
    }

    public bool RunWorkstation(Recipe toProcess) {
        if (ValidNumberOfUsers())
        {
            //TODO send call to another user if needed
            if (resourcesListChanged) {
                toProcess = CheckNeededResources();
                setResourceListChanged(false);
            }
            if (toProcess == null) {
                return false;
            }

            //Maybe need to do something with sync
            lockedResources = true;
            corespondingMinigame.OpenMinigame(this, toProcess);
            return true;

        }
        return false;
    }

    public void SucceedProcessing(Recipe recipe) {
        //Maybe need something else to sync
        if (subscribedUsersIDs[0] == AUtils._playerID) {
            ClearResources();
            lockedResources = false;
            var resultResources = recipe.GetResult();
            foreach (ResourceType resType in resultResources)
            {
                PutResourceType(resType);
            }
        }
    }

    public void FailProcessing()
    {
        //Maybe need something else to sync
        if (subscribedUsersIDs[0] == AUtils._playerID)
        {
            ClearResources();
            lockedResources = false;
        }
    }

    public void CancelProcessing()
    {
        //Call cancelling to other players
        lockedResources = false;
    }

    public void ClearResources()
    {
        inputResources.Clear();
        storage.Clear();
    }

    public void PutResourceType(ResourceType resource)
    {
        if (lockedResources)
        {
            return;
        }

        inputResources.Add(resource);
        AUtils.InsertSortedEnum(storage, resource);
        setResourceListChanged(true);
    }

    public ResourceType PullResourceType()
    {
        if (lockedResources)
        {
            return ResourceType.None;
        }

        if (HaveResource())
        {
            ResourceType pulledResource = inputResources[0];
            storage.Remove(pulledResource);
            inputResources.RemoveAt(0);
            setResourceListChanged(true);

            if (CheckNeededResources() == null)
            {
                UnsubscribeAllUser();
            }

            return pulledResource;
        }

        return ResourceType.None;
    }


    public virtual bool HaveResource()
    {
        return storage.Count > 0;
    }

    public virtual int GetCountOfResources()
    {
        return storage.Count;
    }

    public bool Use()
    {
        Debug.Log("INFO | Used workstation");
        return UseWorkstation();
    }
}
