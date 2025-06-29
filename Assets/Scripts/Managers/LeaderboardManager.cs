using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using System;

namespace Manager{

/// <summary>
/// LeaderboardManager handles the display of player time record for each level scene.
/// </summary>
public class LeaderboardManager : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerLeaderboardData
        {
            public string Id;
            public string Name;
            public string Time;

            public PlayerLeaderboardData(string id, string name, string time)
            {
                Id = id;
                Name = name;
                Time = time;
            }
        }
        [SerializeField] private GameObject leaderboardEntryPrefab;
        [SerializeField] private Transform leaderboardContainer;

        private List<PlayerLeaderboardData> playerDataList;

        void Start()
        {
            playerDataList = new List<PlayerLeaderboardData>();
        }

        public async void UpdateLeaderboard()
        {
            var db = FirebaseDatabase.DefaultInstance;

            string sceneName = SceneLoader.Instance.SceneGroupManager.CurrentSceneGroup.name;
            Debug.Log("Updating leaderboard for scene: " + sceneName);

            // Clear UI cũ
            foreach (Transform child in leaderboardContainer)
                Destroy(child.gameObject);

            playerDataList = new List<PlayerLeaderboardData>();

            var leaderboardRef = db.GetReference("Leaderboards").Child(sceneName);
            var usersRef = db.GetReference("users");

            var leaderboardTask = await leaderboardRef.GetValueAsync();
            if (leaderboardTask == null || !leaderboardTask.Exists)
            {
                Debug.LogWarning("No leaderboard data found.");
                return;
            }

            if (leaderboardTask.Children == null)
            {
                Debug.LogWarning("Leaderboard task Children is null.");
                return;
            }

            foreach (var entry in leaderboardTask.Children)
            {
                string userId = entry.Key;
                string time = entry.Value?.ToString() ?? "0";

                string userName = "Unknown";
                var userSnapshot = await usersRef.Child(userId).Child("userName").GetValueAsync();
                if (userSnapshot.Exists)
                    userName = userSnapshot.Value.ToString();

                playerDataList.Add(new PlayerLeaderboardData(userId, userName, time));
            }

            // Sắp xếp theo thời gian tăng dần (càng nhanh càng tốt)
            playerDataList.Sort((a, b) =>
            {
                float ta = float.TryParse(a.Time, out var fa) ? fa : float.MaxValue;
                float tb = float.TryParse(b.Time, out var fb) ? fb : float.MaxValue;
                return ta.CompareTo(tb);
            });

            foreach (var player in playerDataList)
            {
                GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
                var text = entry.GetComponentInChildren<TMP_Text>();
                if (text != null)
                {
                    if (FirebaseAuth.DefaultInstance.CurrentUser?.UserId == player.Id)
                    {
                        entry.GetComponent<Image>().color = Color.cyan;
                        text.text = $"{player.Name} (You): {Math.Round(float.Parse(player.Time), 2)} sec";
                    }
                    else
                    {
                        text.text = $"{player.Name}: {player.Time} sec";
                    }
                }
            }

            Debug.Log("Leaderboard UI updated successfully.");
        }
    }
}