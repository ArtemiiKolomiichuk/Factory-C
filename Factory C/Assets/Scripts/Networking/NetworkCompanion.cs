using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCompanion : MonoBehaviour
{
    public static bool networkEnabled = false;
    void Start()
    {
        networkEnabled = true;   
    }
}
