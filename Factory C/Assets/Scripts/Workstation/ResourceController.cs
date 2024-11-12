using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; private set; }

    public List<Resource> resourceList;
    public List<Recipe> recipeList;
    public Dictionary<ResourceType, Resource> resourceDictionary = new Dictionary<ResourceType, Resource>();
    public Dictionary<WorkstationType, List<Recipe>> recipeDictionary = new Dictionary<WorkstationType, List<Recipe>>();
    public Dictionary<ResourceType, List<Recipe>> recipeByResource = new Dictionary<ResourceType, List<Recipe>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        foreach (var resource in resourceList)
        {
            resourceDictionary.Add(resource.rType, resource);
        }

        foreach (var recipe in recipeList)
        {
            if (!recipeDictionary.ContainsKey(recipe.workstationType))
            {
                recipeDictionary[recipe.workstationType] = new List<Recipe>();
            }
            recipeDictionary[recipe.workstationType].Add(recipe);

            foreach (var resultResourceType in recipe.resultResources)
            {
                if (!recipeByResource.ContainsKey(resultResourceType))
                {
                    recipeByResource[resultResourceType] = new List<Recipe>();
                }

                recipeByResource[resultResourceType].Add(recipe);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
