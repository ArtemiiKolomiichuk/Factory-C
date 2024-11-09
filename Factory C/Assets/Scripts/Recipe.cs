using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    [Header("Receipt Configuration")]
    public List<ResourceType> inputResources;
    public List<ResourceType> resultResources;
    public WorkstationType workstationType = WorkstationType.None;
    public float difficultyMod;
    public float[] additionalVariables;

    public Recipe(List<ResourceType> inputResources, List<ResourceType> resultResources, WorkstationType workstation, float difficultyMod, float[] additionalVariables)
    {
        this.inputResources = inputResources;
        this.resultResources = resultResources;
        this.workstationType = workstation;
        this.difficultyMod = difficultyMod;
        this.additionalVariables = additionalVariables;
    }

    public bool ThereOnlyNeededResources(List<ResourceType> storage) {
        //TODO add check
        return false;
    }
   
}
