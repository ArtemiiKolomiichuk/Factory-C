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
    public List<Recipe> receipts;
    public List<ResourceType> inputResources;

    public MinigameInterface corespondingMinigame;

    public int numberOfCurrentUsers = 0;
    public int numberOfMaxUsers = 0;
    public bool canUse = true;


    public bool SubscribeUser()
    {
        if (numberOfCurrentUsers < numberOfMaxUsers)
        {
            numberOfCurrentUsers++;
            return true;
        }
        return false;
    }

    public bool UnsubscribeUser() {
        if (numberOfCurrentUsers > 0)
        {
            numberOfCurrentUsers--;
            return true;
        }
        return false;
    }

    public void AddResource(ResourceType resource) {
        //TODO
    }

    public ResourceType PullResource() {
        //TODO
        return ResourceType.None;
    }

    public Recipe CheckNeededResources() {
        //TODO check all receipts
        return null;
    }

    public bool UseWorkstation() {
        if (SubscribeUser()) {
            Recipe toProcess = CheckNeededResources();
            if (toProcess == null) {
                //TODO error mesage
                return false;
            }
            else { 
                corespondingMinigame.OpenMinigame(this, toProcess.difficultyMod);
                return true;
            }

        }
        return false;
    }

    public void SucceedProcessing() { 
        //TODO finilize
    }


    
}
