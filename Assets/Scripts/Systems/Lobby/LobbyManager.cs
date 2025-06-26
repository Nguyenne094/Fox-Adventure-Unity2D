using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Unity.Netcode;
using TMPro;
using UnityEngine.Events;

namespace Network
{
    public class LobbyManager : NetworkSingleton<LobbyManager>
    {
        [Serializable]
        public class PlayerInfo
        {
            public ulong ClientId;
            public string PlayerName;
            public bool IsReady;
        }

        [Header("Lobby UI")]
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_Text lobbyCodeText;

        [Header("Lobby Events")]
        public UnityEvent onAllPlayersReady;

        private Dictionary<string, List<PlayerInfo>> lobbies = new(); // roomCode -> players
        private Dictionary<ulong, string> playerToRoom = new(); // clientId -> roomCode

        public bool JoinSucess { get; private set; }

        // Gọi hàm này sau mỗi thay đổi lobby
        public void SyncLobbyToClients(string code)
        {
            if (!lobbies.ContainsKey(code)) return;
            var players = lobbies[code];
            var names = new Unity.Collections.FixedString128Bytes[players.Count];
            var ready = new byte[players.Count];
            for (int i = 0; i < players.Count; i++)
            {
                names[i] = players[i].PlayerName;
                ready[i] = players[i].IsReady ? (byte)1 : (byte)0;
            }
            var nameArray = new FixedStringArray { Values = names };
            SyncLobbyClientRpc(nameArray, ready);
        }

        // Host tạo lobby, trả về mã phòng
        public string CreateLobby(ulong hostId, string playerName)
        {
            string code = GenerateRoomCode();
            lobbies[code] = new List<PlayerInfo>();
            playerToRoom[hostId] = code;
            return code;
        }

        // Đánh dấu ready
        public void SetReady(ulong clientId, bool ready)
        {
            if (!playerToRoom.ContainsKey(clientId)) return;
            string code = playerToRoom[clientId];
            var player = lobbies[code].Find(p => p.ClientId == clientId);
            if (player != null) player.IsReady = ready;
            SyncLobbyToClients(code);
            TryStartGame(code);
        }

        // Kiểm tra tất cả đã ready
        public bool AllReady(string code)
        {
            if (!lobbies.ContainsKey(code)) return false;
            foreach (var p in lobbies[code])
                if (!p.IsReady) return false;
            return true;
        }

        // Bắt đầu trận nếu tất cả đã ready
        public void TryStartGame(string code)
        {
            if (AllReady(code))
            {
                Debug.Log($"Lobby {code} all ready! Starting game...");
                if (onAllPlayersReady != null)
                    onAllPlayersReady.Invoke();
                // TODO: Load scene hoặc gửi RPC cho tất cả client vào trận
            }
        }

        // Client tham gia lobby bằng mã
        public bool JoinLobby(string code, ulong clientId, string playerName)
        {
            Debug.Log("Abc");
            if (!lobbies.ContainsKey(code))
            {
                Debug.LogWarning($"Lobby {code} does not exist");
                JoinSucess = false;
                return false;
            }
            if (lobbies[code].Exists(p => p.ClientId == clientId))
            {
                Debug.LogWarning($"Client {clientId} already in lobby {code}");
                JoinSucess = false;
                return false;
            }

            lobbies[code].Add(new PlayerInfo { ClientId = clientId, PlayerName = playerName, IsReady = false });
            playerToRoom[clientId] = code;
            SyncLobbyToClients(code);
            JoinSucess = true;
            Debug.Log($"Player {lobbies[code].Count}th: {playerName} joined lobby {code}");
            lobbyCodeText.text = "Lobby Code: " + code;
            Debug.Log($"Lobby {code} now has {lobbies[code].Count} players");
            return true;
        }

        // Xóa player khỏi lobby, xóa lobby nếu không còn ai
        public void RemovePlayerFromLobby(ulong clientId)
        {
            if (!playerToRoom.ContainsKey(clientId)) return;
            string code = playerToRoom[clientId];
            if (lobbies.ContainsKey(code))
            {
                lobbies[code].RemoveAll(p => p.ClientId == clientId);
                if (lobbies[code].Count == 0)
                {
                    DestroyLobby(code);
                }
                else
                {
                    SyncLobbyToClients(code);
                }
            }
            playerToRoom.Remove(clientId);
        }


        #region RPC Methods

        // Gửi thông tin lobby cho tất cả client trong phòng
        [ClientRpc]
        private void SyncLobbyClientRpc(FixedStringArray playerNames, byte[] readyStates)
        {
            // Client: cập nhật UI lobby dựa trên playerNames và readyStates
            var names = playerNames.Values;
            var readyBools = new bool[readyStates.Length];
            for (int i = 0; i < readyStates.Length; i++)
                readyBools[i] = readyStates[i] != 0;
            Debug.Log($"[Client] Sync lobby: " + string.Join(", ", names) + " | Ready: " + string.Join(", ", readyBools));
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestCreateLobbyServerRpc(string playerName, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            var code = CreateLobby(clientId, playerName);
            RequestJoinLobbyServerRpc(code, playerName, rpcParams);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestJoinLobbyServerRpc(string code, string playerName, ServerRpcParams rpcParams = default)
        {
            Debug.Log("Bca");
            ulong clientId = rpcParams.Receive.SenderClientId;
            JoinLobby(code, clientId, playerName);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestSetReadyServerRpc(bool ready, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            SetReady(clientId, ready);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestLeaveLobbyServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            RemovePlayerFromLobby(clientId);
        }

        #endregion

        #region Methods for UI Buttons
        public void OnCreateLobbyButton()
        {
            if (nameInputField == null) { Debug.LogError("Name input field not assigned"); return; }
            string playerName = nameInputField.text;
            if (string.IsNullOrWhiteSpace(playerName)) { Debug.LogWarning("Player name is empty"); return; }
            RequestCreateLobbyServerRpc(playerName);
        }

        public void OnJoinLobbyButton()
        {
            if (codeInputField == null || nameInputField == null) { Debug.LogError("Input field not assigned"); return; }
            string code = codeInputField.text.Trim().ToUpper();
            string playerName = nameInputField.text;
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(playerName))
            {
                Debug.LogWarning("Code or player name is empty");
                return;
            }
            RequestJoinLobbyServerRpc(code, playerName);
        }

        public void OnReadyButton()
        {
            RequestSetReadyServerRpc(true);
        }

        public void OnOutLobbyButton()
        {
            RequestLeaveLobbyServerRpc();
        }
#endregion

        private string GenerateRoomCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var rand = new System.Random();
            string code;
            do
            {
                code = "";
                for (int i = 0; i < 6; i++)
                    code += chars[rand.Next(chars.Length)];
            } while (lobbies.ContainsKey(code));
            return code;
        }
        
        private void DestroyLobby(string code)
        {
            lobbies.Remove(code);
            Debug.Log($"Lobby {code} deleted (empty)");
        }

        private string GetLobbyCode()
        {
            if (playerToRoom.TryGetValue(NetworkManager.LocalClientId, out string code))
                return code;
            return null;
        }
    }
}
