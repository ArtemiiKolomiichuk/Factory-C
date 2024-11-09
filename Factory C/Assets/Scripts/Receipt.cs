using System.Collections.Generic;
using UnityEngine;

public class Receipt : MonoBehaviour
{
    [Header("Receipt Configuration")]
    public List<ResourceType> inputResources;
    public List<ResourceType> resultResources;
    public float difficultyMod;
    public float[] additionalVariables;

    public Receipt(List<ResourceType> inputResources, List<ResourceType> resultResources, float difficultyMod, float[] additionalVariables)
    {
        this.inputResources = inputResources;
        this.resultResources = resultResources;
        this.difficultyMod = difficultyMod;
        this.additionalVariables = additionalVariables;
    }

    public bool ThereOnlyNeededResources(List<ResourceType> storage) {
        //TODO add check
        return false;
    }
   
}
