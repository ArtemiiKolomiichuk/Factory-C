using System;
using System.Collections.Generic;
using UnityEngine;

public enum WorkstationType
{
    None,
    Forge,
    Slime,
    Furnace
}

public class Workstation : MonoBehaviour
{
    [Header("Workstation Configuration")]
    public WorkstationType workstationType;
    public List<Recipe> recipes;

    public List<ResourceType> inputResources;
    public List<ResourceType> storage; //for sorted input array

    public MinigameInterface corespondingMinigame;

    public List<int> subscribedUsersIDs = new List<int>();
    public int numberOfCurrentUsers = 0;
    public int numberOfMaxUsers = 0;
    //public bool canUse = true; not needed

    public Transform resourceSpawnPoint;

    private bool resourcesListChanged = false;
    private bool lockedResources = false;

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

    public void ClearResources()
    {
        inputResources.Clear();
        storage.Clear();
    }

    public void AddResource(ResourceType resource) {
        if (lockedResources) {
            return;
        }

        inputResources.Add(resource);
        AUtils.InsertSortedEnum(storage, resource);
        resourcesListChanged = true;
    }

    //Can be used for physical objects
    public void AddResourceObject(Resource resource)
    {
        AddResource(resource.rType);
        //TODO Call from resource it physicall deletion
    }

    public ResourceType PullResource()
    {
        if (lockedResources)
        {
            return ResourceType.None;
        }

        if (inputResources.Count > 0)
        {
            ResourceType pulledResource = inputResources[0];
            storage.Remove(pulledResource);
            inputResources.RemoveAt(0);
            resourcesListChanged = true;

            if (CheckNeededResources() == null) {
                UnsubscribeAllUser();
            }

            return pulledResource;
        }

        return ResourceType.None;
    }

    public ResourceType PullResourceObject()
    {
        ResourceType type = PullResource();
        if (resourceSpawnPoint != null && type != ResourceType.None) {
            if (!SpawnResource(type)) { 
                return ResourceType.None;
            }
        }

        return type;
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
            resourcesListChanged = false;

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
                resourcesListChanged = false;
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

    private bool SpawnResource(ResourceType type) {
        //TODO Spawn physical object on transform
        return false;
    }

    public void SucceedProcessing(Recipe recipe) {
        //Maybe need something else to sync
        if (subscribedUsersIDs[0] == AUtils._playerID) {
            ClearResources();
            lockedResources = false;
            foreach (ResourceType resType in recipe.resultResources)
            {
                AddResource(resType);
            }
        }
    }

    public void CancelProcessing()
    {
        //Call cancelling to other players
        lockedResources = false;
    }



}
