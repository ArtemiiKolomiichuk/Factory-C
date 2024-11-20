using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeatSpawner : NetworkBehaviour
{

    public GameObject playerPrefab;

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
                Debug.Log("Binding set ");
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            }
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected: " + clientId);
        if (IsServer)
        {
            StartCoroutine(SpawnPlayerForClient(clientId));
        }
    }

    private IEnumerator SpawnPlayerForClient(ulong clientId)
    {
        yield return new WaitForSeconds(3);
        Debug.Log($"(has: {NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject})Spawning player for client " + clientId);
        if(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject == null)
        {
            var playerPrefab = Instantiate(this.playerPrefab, new Vector3(-40.8348579f, 3.69000006f, -5.05999994f), Quaternion.identity);
            playerPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
    }
}
