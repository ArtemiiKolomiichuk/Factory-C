using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeatSpawner : NetworkBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneLoaded;   
    }

    private void SceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.name == LobbyController.TargetScene)
        {
            if(IsServer)
            {
                var go =PrefabSystem.GetByIndex(0);
                go = Instantiate(go, new Vector3(-30.8348579f, 3.69000006f, -5.05999994f), Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
