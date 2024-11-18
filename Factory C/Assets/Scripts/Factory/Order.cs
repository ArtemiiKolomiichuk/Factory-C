using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Order")]
public class Order : ScriptableObject
{
    public Resource resource;
    public uint durationInGameMinutes;
}
