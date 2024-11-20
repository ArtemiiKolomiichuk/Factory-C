using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDisplayer : MonoBehaviour
{

    public Dictionary<ResourceType, GameObject> resources = new Dictionary<ResourceType, GameObject>();
    private ResourceType currentlyShown = ResourceType.None;


    void Start()
    {
        foreach (Transform child in transform)
        {
            GetRType rTypeComponent = child.GetComponent<GetRType>();
            if (rTypeComponent != null)
            {
                resources.Add(rTypeComponent.resourceType, child.gameObject);
            }
            else
            {
                Debug.LogWarning($"No GetRType component found on {child.name}");
            }
        }

        foreach (var resource in resources.Values)
        {
            if (resource != null)
            {
                resource.SetActive(false);
            }
        }
    }

    public void ShowResource(ResourceType type) 
    {
        HideResourceAction(currentlyShown);
        if (type != ResourceType.None)
        {
            ShowResourceAction(type);
        }
    }

    private void ShowResourceAction(ResourceType type) 
    {
        currentlyShown = type;
        resources[currentlyShown].SetActive(true);
    }

    private void HideResourceAction(ResourceType type) 
    {
        if (type != ResourceType.None)
        {
            resources[type].SetActive(false);
        }
        currentlyShown = ResourceType.None;
    }

}
