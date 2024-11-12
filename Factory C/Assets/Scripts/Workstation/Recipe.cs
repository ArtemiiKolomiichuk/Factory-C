using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe Data", order = 51)]
public class Recipe : ScriptableObject
{
    [Header("Receipt Configuration")]
    public List<ResourceType> inputResources;
    public List<ResourceType> resultResources;
    public WorkstationType workstationType = WorkstationType.None;
    public float difficultyMod;
    public float[] additionalVariables;

    //Maybe change on Start Or Awake if it will irritate
    private void OnValidate()
    {
        inputResources.Sort();
        resultResources.Sort();
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
