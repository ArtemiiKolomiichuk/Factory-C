using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseToCleanStorage : MonoBehaviour, UsableInterface
{
    public GameObject resourceHolderObject = null;
    public UniverslaResourceHolderInterface resourceHolder = null;

    private void Start()
    {
        if (resourceHolderObject != null)
        {
            resourceHolder = resourceHolderObject.GetComponent<UniverslaResourceHolderInterface>();
        }
    }

    public bool Use()
    {
        resourceHolder.ClearResources();
        return true;
    }
}
