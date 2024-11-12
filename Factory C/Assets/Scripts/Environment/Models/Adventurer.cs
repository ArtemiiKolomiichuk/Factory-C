using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public float Speed { get; set; } = 2.0f;
    public BehaviorType Behavior { get; set; }
    public List<Interactable> Items { get; set; }

    // Debug placeholder methods
    public void StartRaid() { Debug.Log("Adventurer starts raid."); }
    public void EndRaid() { Debug.Log("Adventurer ends raid."); }
    public void Attack(Monster monster) { Debug.Log("Adventurer attacks monster."); }
}