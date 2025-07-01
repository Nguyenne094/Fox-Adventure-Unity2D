using UnityEngine;

namespace Quest
{
    public enum QuestType { Level, Global }

    [CreateAssetMenu(menuName = "Quest/QuestData")]
    public class QuestDataSO : ScriptableObject
    {
        public string questId;
        public QuestType type;
        public string description;

        [Header("Level Quest")]
        [Tooltip("Time in seconds")] public float maxTime;
        public int maxHits;
        public int requiredItemCount;
    }
}