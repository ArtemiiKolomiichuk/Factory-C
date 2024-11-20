using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RandomRecipeOutput
{
    public ResourceType rType;
    public float probability;

    public RandomRecipeOutput(ResourceType rType, float probability)
    {
        this.rType = rType;
        this.probability = probability;
    }
}



[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe Data", order = 51)]
public class Recipe : ScriptableObject
{
    [Header("Receipt Configuration")]
    public List<ResourceType> inputResources = new List<ResourceType>();
    public List<ResourceType> resultResources = new List<ResourceType>();
    public List <RandomRecipeOutput> randomResources = new List<RandomRecipeOutput>();

    public WorkstationType workstationType = WorkstationType.None;
    public float difficultyMod;
    public float[] additionalVariables;

    //Maybe change on Start Or Awake if it will irritate
    private void OnValidate()
    {
        inputResources.Sort();
        resultResources.Sort();
    }

    public List<ResourceType> GetResult() 
    {
        List<ResourceType> result = new List<ResourceType>(resultResources);
        foreach (var rr in randomResources)
        {
            if (AUtils.IsRandomSuccess(rr.probability)) 
            {
                result.Add(rr.rType);
            }
        }
        return result;
    }

    public bool ThereOnlyNeededResources(List<ResourceType> storage)
    {
        //Storage need to be sorted or sort there
        List<ResourceType> sortedStorage = storage;
        //sortedStorage = new List<ResourceType>(storage);
        //sortedStorage.Sort();

        if (inputResources.Count != sortedStorage.Count)
        {
            return false;
        }

        for (int i = 0; i < inputResources.Count; i++)
        {
            if (inputResources[i] != sortedStorage[i])
            {
                return false;
            }
        }

        return true;
    }

}
