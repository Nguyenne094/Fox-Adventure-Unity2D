using Nguyen.Event;
using UnityEngine;
using Utils;

namespace Quest
{
    public class LevelQuestManager : MonoBehaviour
    {
        [SerializeField] private QuestDataSO currentQuest;
        [SerializeField] private VoidEventChannelSO onPlayerWin;
        [SerializeField] private LevelTimer levelTimer;
        [SerializeField] private HitTracker hitTracker;
        [SerializeField] private SpecialItemCollector itemCollector;

        private FirebaseQuestManager firebaseQuestManager;

        private void Awake()
        {
            firebaseQuestManager = GetComponent<FirebaseQuestManager>();
            levelTimer ??= FindFirstObjectByType<LevelTimer>();
            hitTracker ??= FindFirstObjectByType<HitTracker>();
            itemCollector ??= FindFirstObjectByType<SpecialItemCollector>();
        }

        void OnEnable()
        {
            onPlayerWin.OnEventRaised += HandlePlayerWin;
        }

        void OnDisable()
        {
            onPlayerWin.OnEventRaised -= HandlePlayerWin;
        }

        private void HandlePlayerWin()
        {

            float timeTaken = levelTimer.ElapsedTime;
            int hitsTaken = hitTracker.HitCount;
            int itemsCollected = itemCollector.SpecialItemCount;

            firebaseQuestManager.UpdateLevelQuest(currentQuest, timeTaken, hitsTaken, itemsCollected);
        }
    }
}