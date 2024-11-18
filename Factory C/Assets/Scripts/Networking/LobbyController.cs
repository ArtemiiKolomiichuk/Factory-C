using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;
    public Lobby lobby;

    private const float updateInterval = 3.0f;

    public const string TargetScene = "MainGame";
    private async void Awake()
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

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task CreateLobby()
    {
        string lobbyName = "new lobby";
        int maxPlayers = 5;
        CreateLobbyOptions options = new()
        {
            IsPrivate = true,
            Player = new Player(
                AuthenticationService.Instance.PlayerId, 
                data: new Dictionary<string, PlayerDataObject> 
                { 
                    { "PlayerName", new PlayerDataObject( PlayerDataObject.VisibilityOptions.Member, AuthenticationService.Instance.PlayerName ?? "Player") }
                }
            ),
            Data = new Dictionary<string, DataObject>
            {
                { "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, "") }
            }
        };
        lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        StartCoroutine(UpdateLobby());
    }

    public async void StartGame()
    {
        var code = await RelayController.Instance.StartHostWithRelay();
        if(string.IsNullOrEmpty(code)) return;

        var options = new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                { "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, code) }
            }
        };
        await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
        onLobbyUpdated = null;
        SceneManager.LoadScene(TargetScene);
    }

    public async Task<bool> Join(string code)
    {
        try
        {
            JoinLobbyByCodeOptions options = new()
            {
                Player = new Player(
                    AuthenticationService.Instance.PlayerId,
                    data: new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, AuthenticationService.Instance.PlayerName ?? "Player") }
                    }
                )
            };
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
            StartCoroutine(UpdateLobby());
            return lobby != null;
        }
        catch (Exception e)
        {
            if(e.Message == "player is already a member of the lobby")
            {
                var lobbyIds = await LobbyService.Instance.GetJoinedLobbiesAsync();
                foreach(var id in lobbyIds)
                {
                    await Task.Delay(500);
                    lobby = await LobbyService.Instance.GetLobbyAsync(id);
                    if(lobby.LobbyCode != code)
                    {
                        await Task.Delay(500);
                        await LobbyService.Instance.RemovePlayerAsync(id, AuthenticationService.Instance.PlayerId);
                    }
                }
                await Task.Delay(500);
                lobbyIds = await LobbyService.Instance.GetJoinedLobbiesAsync();
                await Task.Delay(500);
                lobby = lobbyIds.Count != 1 ? null : await LobbyService.Instance.GetLobbyAsync(lobbyIds[0]);
                if(lobby == null)
                {
                    try
                    {
                        JoinLobbyByCodeOptions options = new()
                        {
                            Player = new Player(
                                AuthenticationService.Instance.PlayerId,
                                data: new Dictionary<string, PlayerDataObject>
                                {
                                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, AuthenticationService.Instance.PlayerName ?? "Player") }
                                }
                            )
                        };
                        lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (lobby != null)
                {
                    StartCoroutine(UpdateLobby());
                }
                return lobby != null;
            }
            return false;
        }
    }

    public string LobbyCode()
    {
        return lobby?.LobbyCode; 
    }

    public async void LeaveLobby()
    {
        StopAllCoroutines();
        onLobbyUpdated = null;
        await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
    }

    public Action onLobbyUpdated;
    private IEnumerator UpdateLobby()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            if (lobby != null)
            {
                UpdateLobbyAsync();
            }
            if(lobby?.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);
            }
        }
    }

    private async void UpdateLobbyAsync()
    {
        Lobby _lobby = await LobbyService.Instance.GetLobbyAsync(lobby.Id);
        if (lobby.HostId != AuthenticationService.Instance.PlayerId)
        {
            string relayId = _lobby.Data["RelayCode"].Value;
            if(!string.IsNullOrEmpty(relayId))
            {
                bool starting = await RelayController.Instance.StartClientWithRelay(relayId);
                if(starting)
                {
                    SceneManager.LoadScene(TargetScene);
                    StopAllCoroutines();
                    return;
                }
            }
        }
        lobby = _lobby;
        onLobbyUpdated?.Invoke();
    }
}
