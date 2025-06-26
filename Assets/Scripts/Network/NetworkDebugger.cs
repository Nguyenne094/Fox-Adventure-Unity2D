using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class NetworkDebugger : MonoBehaviour
    {
        private NetworkManager m_NetworkManager;

        private void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();
        }

        void Start()
        {
            // Kiểm tra trạng thái mạng khi khởi động
            if (!m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
            {
                // Nếu chưa có host nào, start host
                m_NetworkManager.StartHost();
            }
            else if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
            {
                // Nếu đã có host/server, start client
                m_NetworkManager.StartClient();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(100, 10, 300, 300));
            if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        private void StartButtons()
        {
            if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
            if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
            if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
        }

        private void StatusLabels()
        {
            var mode = m_NetworkManager.IsHost ?
                "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        public void StopNetwork()
        {
            m_NetworkManager.Shutdown();
        }
    }
}