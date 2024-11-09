using UnityEngine;

public enum ResourceType
{
    None,
    Wood,
    Stone,
    Metal,
    Food,
    Water
}

public class Resource : ScriptableObject
{
    public ResourceType Type;

    public float weight;

    public static GameObject ResourcePrefab;

    public bool isHolding = false;

    public void Initialize(ResourceType type, float weight = 1f)
    {
        Type = type;
        this.weight = weight;
        UpdateResourceVisual();
    }

    private void UpdateResourceVisual()
    {
        //TODO if needed
    }

    public void SetIsHolding(bool state) {
        isHolding = state;
    }
}
