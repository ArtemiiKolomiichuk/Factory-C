using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;
    public Lobby lobby;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private async void Start()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public async void CreateLobby()
    {
        string lobbyName = "new lobby";
        int maxPlayers = 5;
        CreateLobbyOptions options = new()
        {
            IsPrivate = true
        };
        lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
    }

    public async Task<bool> Join(string code)
    {
        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            return lobby != null;
        }
        catch 
        {
            return false;
        }
    }

    public string LobbyCode()
    {
        return lobby?.LobbyCode; 
    }

    public async void LeaveLobby()
    {
        await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
    }
}
