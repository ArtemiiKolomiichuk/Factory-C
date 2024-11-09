using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Speed { get; set; } = 1.5f;
    public BehaviorType Behavior { get; set; }
    public bool IsAggressive { get; set; } = false;
    public List<Interactable> Items { get; set; }
}
