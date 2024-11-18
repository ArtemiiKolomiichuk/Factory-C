using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Customer))]
public class CustomerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Customer customer = (Customer)target;
        if (GUILayout.Button("Go Home")) {
            customer.GoHome();
        }
    }
}
#endif