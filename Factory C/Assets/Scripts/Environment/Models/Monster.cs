using Assets.Scripts.ItemsScripts;
using System.Collections.Generic;
using Assets.Scripts.ItemsScripts;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Speed { get; set; } = 1.5f;
    public BehaviorType Behavior { get; set; }
    public bool IsAggressive { get; set; } = false;
    public List<Interactable> Items { get; set; }

    // Debug placeholder methods
    public void Attack(Adventurer adventurer) { Debug.Log("Monster attacks adventurer."); }

}
