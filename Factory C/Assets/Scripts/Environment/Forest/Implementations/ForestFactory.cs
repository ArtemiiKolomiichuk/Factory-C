using UnityEngine;

public class ForestFactory : MonoBehaviour, IForestFactory
{
    public GameObject monsterPrefab;

    public Monster CreateMonster(Vector3 spawnPosition)
    {
        GameObject monsterObj = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        Monster monster = monsterObj.GetComponent<Monster>();
        AssignResources(monster);
        return monster;
    }

    public void AssignResources(Monster monster)
    {
        monster.Resources = new List<Item> { new Item("Monster Fur"), new Item("Monster Claw") };
        Debug.Log("Resources assigned to monster.");
    }
}
