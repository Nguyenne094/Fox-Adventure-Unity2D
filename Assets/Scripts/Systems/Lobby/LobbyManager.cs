using UnityEngine;
using System;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Lobbies;
// using Unity.Services.Lobbies.Models;

public class LobbyManager : MonoBehaviour
{
    [Header("Lobby Settings")]
    [SerializeField] private int _maxPlayers = 4;
    [SerializeField] private string _lobbyName = "MyLobby";

    public async void InitializationOptions()
    {
        try
        {
            // Khởi tạo Unity Services để sử dụng các dịch vụ như Authentication, Lobby, v.v.
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}\n{e.InnerException}");
        }
    }

    public async void SignIn()
    {
        try
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in successfully as " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to sign in: {e.Message}");
            return;
        }
    }

    public async void Login()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Logged in successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to login: {e.Message}");
            return;
        }
    }

    public async void CreateLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, _maxPlayers);
            Debug.Log($"Lobby created: {lobby.Name} with ID: {lobby.Id}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create lobby: {e.Message}");
            return;
        }
    }

    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log($"Joined lobby: {lobby.Name}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join lobby: {e.Message}");
            return;
        }
    }

    public void ListLobbies()
    {
        try
        {
            QueryResponse lobbyList = LobbyService.Instance.QueryLobbiesAsync().Result;
            Debug.Log($"Found {lobbyList.Results.Count} lobbies:");
            foreach (var lobby in lobbyList.Results)
            {
                Debug.Log($"Lobby: {lobby.Name}, ID: {lobby.Id}, Players: {lobby.Players.Count}/{lobby.MaxPlayers}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to list lobbies: {e.Message}");
            return;
        }
    }
}