using System;
using Nguyen.Event;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Manager
{
    /// <summary>
    /// Manage Win Lose Event
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public VoidEventChannelSO playerWinEventChannel;
        public VoidEventChannelSO playerLoseEventChannel;
    }
}