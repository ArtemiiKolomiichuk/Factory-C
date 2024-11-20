using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, Interactible_v2
{
    public GameObject toDestoroy = null;
    public ResourceType rType;

    void Start()
    {
        toDestoroy = GetTopmostParent();
    }

    public GameObject GetTopmostParent()
    {
        Transform current = transform;

        while (current.parent != null)
        {
            current = current.parent;
        }

        return current.gameObject;
    }

    public object Interact(KeyCode key, object data) //in data PlayerInventory (SingleItemInventory)
    {
        if (key == KeyCode.P) 
        {
            SingleItemInventory inv = ((SingleItemInventory)data);
            if (inv.HaveFreeSpace()) 
            {
                inv.PutResourceType(rType);
                Destroy(toDestoroy);
                return true;
            }
        }

        return false;
    }

    private void OnDestroy()
    {
        selfDestroy = true;
        foreach (var owner in owners) 
        {
            owner.RemoveFromTargets(this);
        }
    }

    private bool selfDestroy = false;
    private List<UserInteraction> owners = new List<UserInteraction>();

    public virtual void BehaveOnTargeted(object data) //if needed outline 
    {
        owners.Add((UserInteraction)data);
        Debug.Log("PICKABLE | targeted " + owners.Count + " | " + toDestoroy.ToString());
    }

    public virtual void BehaveOnUnTargeted(object data) 
    {
        if (!selfDestroy)
        {
            owners.Remove((UserInteraction)data);
            Debug.Log("PICKABLE | UNtargeted " + owners.Count + " | " + toDestoroy.ToString());
        }
    }
}
