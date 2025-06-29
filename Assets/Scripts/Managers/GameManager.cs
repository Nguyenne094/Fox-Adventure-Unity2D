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
        public UserData CurrentUserData { get => userInfor; set => userInfor = value; }

        private UserData userInfor;

        void Start()
        {
            CurrentUserData = new();
        }
    }
}