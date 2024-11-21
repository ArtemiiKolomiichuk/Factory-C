using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static GameController Instance { get; private set; }

    private bool isGameOver = false;
    public Action GameOverAction;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (!IsServer) return; // Ensure it's executed on the server
        if (!NetworkObject.IsSpawned)
        {
            NetworkObject.Spawn();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGameOver() {
        return isGameOver;
    }

    public void GameOver() {
        isGameOver = true;
        GameOverAction?.Invoke();
        GameOverClientRpc();
    }

    [ClientRpc]
    public void GameOverClientRpc() {
        GameOverUIController.Instance.OnGameOver();
    }
}
