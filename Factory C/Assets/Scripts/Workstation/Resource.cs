using UnityEngine;

public enum ResourceType
{
    None,

    Wood, //Legacy VVV
    Stone,
    Food,
    Water, //Legacy AAA
    
    SpiderCorpse,
    VenomousFangs,
    PoisonousArrows,
    TreantCorpse,
    Branch,
    Sword,
    Metal,
    AdventurerCorpseSword,
    BrokenSword,
    AdventurerCorpseHelmet,
    DamagedHelmet,
    AdventurerCorpseBreastplate,
    DamagedBreastplate,
    AdventurerCorpseBottle,
    EmptyBottle,
    BonesAndSkull,
    BoneHelmet, //Horned helmet or Skull helmet
    TrollHeart,
    HealingPotion,
    WolfPelt,
    WolfArmour,

    AdventurerCorpse,
}

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource Data", order = 51)]
public class Resource : ScriptableObject
{
    public ResourceType rType;

    public float weight;

    public static GameObject ResourcePrefab;

    public bool isHolding = false;

    public void Initialize(ResourceType type, float weight = 1f)
    {
        this.rType = type;
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
