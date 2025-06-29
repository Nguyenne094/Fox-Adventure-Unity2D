using UnityEngine;
using Nguyen.Event;
using Utils;

namespace Manager
{
    /// <summary>
    /// Manage Win Lose Event
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {

        [Header("Events")]
        public VoidEventChannelSO playerWinEventChannel;
        public VoidEventChannelSO playerLoseEventChannel;

        public bool IsPlayerWin { get; set; }
        public bool IsPlayerLose { get; set; }
        public UserInfor UserInfo { get => userInfor; set => userInfor = value; }

        private UserInfor userInfor;

        void Start()
        {
            UserInfo = new();
        }
    }
}

[System.Serializable]
public struct UserInfor
{
    public string userId;
    public string userName;
    public string email;
    public string password;
}