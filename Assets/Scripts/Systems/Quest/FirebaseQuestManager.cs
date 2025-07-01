using Firebase.Database;
using Firebase.Auth;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace Quest
{
    [System.Serializable]
    public class QuestProgress
    {
        public string questId;
        public bool completed;
        public int starsEarned;
        public float bestTime;
        public int maxHitsTaken;
        public int itemsCollected;
    }

    public class FirebaseQuestManager : Singleton<FirebaseQuestManager>
    {
        public List<QuestDataSO> allQuests;
        public Dictionary<string, QuestProgress> progressDict = new();

        private DatabaseReference dbRef;
        private string userId;

        public override void Awake()
        {
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        }

        public void LoadProgress()
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).Child("quests")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var child in snapshot.Children)
                    {
                        var json = child.GetRawJsonValue();
                        var progress = JsonConvert.DeserializeObject<QuestProgress>(json);
                        progressDict[progress.questId] = progress;
                    }
                }
            });
        }

        public void UpdateLevelQuest(QuestDataSO data, float time, int hits, int itemCount)
        {
            var prog = new QuestProgress
            {
                questId = data.questId,
                bestTime = time,
                maxHitsTaken = hits,
                itemsCollected = itemCount,
                starsEarned = CalculateStars(data, time, hits, itemCount),
                completed = true
            };

            progressDict[data.questId] = prog;
            SaveProgress(prog);
        }

        public bool IsLevelQuestCompleted(QuestDataSO data)
        {
            return progressDict.TryGetValue(data.questId, out var progress) && progress.completed;
        }

        private void SaveProgress(QuestProgress progress)
        {
            string json = JsonConvert.SerializeObject(progress);
            dbRef.Child("users").Child(userId).Child("quests").Child(progress.questId).SetRawJsonValueAsync(json);
        }

        private int CalculateStars(QuestDataSO data, float time, int hits, int itemCount)
        {
            int stars = 0;
            if (time <= data.maxTime) stars++;
            if (hits <= data.maxHits) stars++;
            if (itemCount >= data.requiredItemCount) stars++;
            return stars;
        }
    }
}