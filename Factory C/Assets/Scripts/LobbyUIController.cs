using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.Services.Authentication;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private Button hostButton;

    [SerializeField] private TMP_InputField joinCodeInputField;

    [SerializeField] private GameObject lobbyUI;
    private Transform LobbyBackground => lobbyUI.transform.GetChild(0);
    private TextMeshProUGUI LobbyCode => LobbyBackground.transform.Find("JoinCode").GetComponent<TextMeshProUGUI>();
    private GameObject PlayerList => LobbyBackground.transform.Find("Players").gameObject;
    private GameObject Player => PlayerList.transform.GetChild(0).gameObject;
    private Button StartButton => LobbyBackground.transform.Find("Start").GetComponent<Button>();


    private void Start()
    {
        joinButton.onClick.AddListener(JoinButtonClicked);
        hostButton.onClick.AddListener(HostButtonClicked);
        if(AuthenticationService.Instance.PlayerName is string name)
        {
            GameObject.Find("PlayerName").GetComponent<TMP_InputField>().text = name[..^5];
        }
        GameObject.Find("PlayerName").GetComponent<TMP_InputField>().onEndEdit.
            AddListener((s) => AuthenticationService.Instance.UpdatePlayerNameAsync(s));
    }

    private async void JoinButtonClicked()
    {
        string joinCode = joinCodeInputField.text;
        if (string.IsNullOrEmpty(joinCode) || joinCode.Trim().Length != 6)
        {
            Debug.LogError("Join code is wrong");
            return;
        }

        if(await LobbyController.Instance.Join(joinCode))
            ShowLobbyUI();
    }

    private void ShowLobbyUI()
    {
        lobbyUI.SetActive(true);
        if (LobbyController.Instance.lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            StartButton.gameObject.SetActive(true);
            StartButton.onClick.AddListener(() => LobbyController.Instance.StartGame());
        }
        else
        {
            StartButton.gameObject.SetActive(false);
        }
        LobbyController.Instance.onLobbyUpdated += UpdateLobbyUI;
        UpdateLobbyUI();
    }

    public void UpdateLobbyUI()
    {
        for(int i = PlayerList.transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(PlayerList.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < LobbyController.Instance.lobby.Players.Count; i++)
        {
            if(i == 0)
            {
                Player.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = 
                    $"{LobbyController.Instance.lobby.Players[i].Data["PlayerName"].Value} (HOST)";
            }
            else
            {
                GameObject player = Instantiate(Player, PlayerList.transform);
                player.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = 
                    LobbyController.Instance.lobby.Players[i].Data["PlayerName"].Value;
            }
        }
        LobbyCode.text = $"Code: {LobbyController.Instance.LobbyCode()}";
    }

    private async void HostButtonClicked()
    {
        await LobbyController.Instance.CreateLobby();
        ShowLobbyUI();
    }
}
