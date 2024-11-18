using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSystem : MonoBehaviour
{
    public static PrefabSystem Instance { get; private set; }

    [SerializeField]
    private GameObject[] itemPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject FindItem(Resource itemData)
    {
        if (Instance == null)
        {
            Debug.LogError("PrefabSystem instance is not set!");
            return null;
        }

        foreach (var a in Instance.itemPrefabs)
        {
            if (a.GetComponent<ItemPickUp>()!= null && a.GetComponent<ItemPickUp>().ItemData.rType == itemData.rType)
            {
                return a;
            }
            
        }

        return null;
    }
    public static GameObject GetMask()
    {
        if (Instance == null)
        {
            Debug.LogError("PrefabSystem instance is not set!");
            return null;
        }
        return Instance.itemPrefabs[0];
    }
}
