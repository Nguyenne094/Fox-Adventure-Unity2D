using Nguyen.Event;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Manager
{
    /// <summary>
    /// Manage Win Lose Event
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameObject m_PlayerPrefab;
        [SerializeField] private Transform m_SpawnPoint;
        public VoidEventChannelSO playerWinEventChannel;
        public VoidEventChannelSO playerLoseEventChannel;

        public override void Awake() {
            base.Awake();
            if (m_PlayerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned in GameManager!");
            }
        }

        void Start()
        {
            SpawnPlayerServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
        {
            if (m_PlayerPrefab != null)
            {
                GameObject obj = Instantiate(m_PlayerPrefab, m_SpawnPoint.position, Quaternion.identity);
                obj.GetComponent<NetworkObject>().Spawn();
                Debug.Log("Player spawned at " + m_SpawnPoint.position);
            }
            else
            {
                Debug.LogError("Player prefab is not assigned!");
            }
        }
    }
}