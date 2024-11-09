using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public float Speed { get; set; } = 2.0f;
    public BehaviorType Behavior { get; set; }
    public List<Interactable> Items { get; set; }
}