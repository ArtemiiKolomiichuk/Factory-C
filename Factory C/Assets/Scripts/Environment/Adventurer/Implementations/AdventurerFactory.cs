using UnityEngine;

public class AdventurerFactory : MonoBehaviour, IAdventurerFactory
{
    public GameObject adventurerPrefab;

    public Adventurer CreateAdventurer(Vector3 spawnPosition)
    {
        GameObject adventurerObj = Instantiate(adventurerPrefab, spawnPosition, Quaternion.identity);
        Adventurer adventurer = adventurerObj.GetComponent<Adventurer>();
        AssignLoot(adventurer);
        return adventurer;
    }

    public void AssignLoot(Adventurer adventurer)
    {
        adventurer.Loot = new List<Item> { new Item("Gold Coin"), new Item("Healing Potion") };
        Debug.Log("Loot assigned to adventurer.");
    }
}
