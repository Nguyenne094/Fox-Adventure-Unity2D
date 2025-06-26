using Network;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LobbyManager))]
public class LobbyUtilities : MonoBehaviour
{
    [SerializeField] private UnityEvent onCreateOrJoinLobbySuccess;

    private LobbyManager lobbyManager;

    private void Awake()
    {
        lobbyManager = GetComponent<LobbyManager>();
    }

    public void OnCreateOrJoinLobbySuccess()
    {
        if (lobbyManager.JoinSucess)
        {
            // This method can be called when a lobby is successfully created or joined
            onCreateOrJoinLobbySuccess?.Invoke();
        }
    }
}