using UnityEngine;
using Nguyen.Event;
using Utils;
using TMPro;

namespace Manager
{
    /// <summary>
    /// Manage Win Lose Event
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("References")]
        [SerializeField] private LevelStarCriteria starCriteria;
        [SerializeField] private LevelTimer levelTimer;
        [SerializeField] private TMP_Text starResultText;

        [Header("Events")]
        public VoidEventChannelSO playerWinEventChannel;
        public VoidEventChannelSO playerLoseEventChannel;

        public bool IsPlayerWin { get; set; }
        public bool IsPlayerLose { get; set; }

        private int collectedStar;
        private int starsEarned;
        private bool tookDamage;

        void Start()
        {
            levelTimer.StartTimer();
            EvaluateStars();
        }

        public void EvaluateStars()
        {
            starsEarned = 0;

            if (collectedStar >= starCriteria.requiredStarsToCollect)
                starsEarned++;

            if (!starCriteria.requireNoDamage)
            {
                starsEarned++;
            }
            else if (starCriteria.requireNoDamage && !tookDamage)
            {
                starsEarned++;
            }

            if (levelTimer.GetElapsedTime() <= starCriteria.timeLimitInSeconds)
                starsEarned++;

            starResultText.text = $"Stars Earned: {starsEarned}/3";
        }

        public void CollectStar(int starValue)
        {
            collectedStar += starValue;
            EvaluateStars();
        }

        public void TakeDamage()
        {
            tookDamage = true;
            EvaluateStars();
        }
    }
}