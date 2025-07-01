using Nguyen.Event;
using UnityEngine;

namespace Quest
{
    public class LevelEndTrigger : MonoBehaviour
    {
        public QuestDataSO levelQuest;
        public float timeTaken;
        public int hitsTaken;
        public int itemsCollected;

        public VoidEventChannelSO onLevelCompleted;

        public void CompleteLevel()
        {
            FirebaseQuestManager.Instance.UpdateLevelQuest(levelQuest, timeTaken, hitsTaken, itemsCollected);
            onLevelCompleted.RaiseEvent();
        }
    }
}