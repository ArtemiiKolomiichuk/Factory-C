using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
