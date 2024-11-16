using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CustomerSpawner))]
public class CustomerSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CustomerSpawner spawner = (CustomerSpawner)target;
        if (GUILayout.Button("Spawn")) {
            spawner.SpawnCustomer();
        }
        if (GUILayout.Button("Go home")) {
            spawner.GoHomeCustomer();
        } 
    }
}
#endif