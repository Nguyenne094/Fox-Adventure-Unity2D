using Unity.Netcode;

namespace Network
{
    public class PlayerLobbyState : NetworkBehaviour
    {
        public NetworkVariable<bool> IsReady = new(false);
        public NetworkVariable<string> PlayerName = new("");

        public void SetReady(bool ready)
        {
            if (IsOwner)
                SetReadyServerRpc(ready);
        }

        public void SetName(string name)
        {
            if (IsOwner)
                SetNameServerRpc(name);
        }

        // Gọi từ client để yêu cầu server cập nhật trạng thái ready
        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(bool ready)
        {
            IsReady.Value = ready;
        }

        // Gọi từ client để yêu cầu server cập nhật tên
        [ServerRpc(RequireOwnership = false)]
        private void SetNameServerRpc(string name)
        {
            PlayerName.Value = name;
        }
    }
}
