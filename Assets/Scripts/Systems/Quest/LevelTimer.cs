using UnityEngine;
using UnityEngine.Events;

namespace Quest
    {
    public class LevelTimer : MonoBehaviour
    {
        public UnityEvent<float> OnTimeUpdated; // Sự kiện cập nhật thời gian
        public UnityEvent OnTimeLimitReached; // Sự kiện khi hết thời gian

        private bool isRunning;
        public float ElapsedTime { get; private set; }

        public void StartTimer()
        {
            ElapsedTime = 0f;
            isRunning = true;
        }

        public void PauseTimer()
        {
            isRunning = false;
        }

        public void ResumeTimer()
        {
            isRunning = true;
        }

        public void ResertTimer()
        {
            ElapsedTime = 0f;
            isRunning = false;
            OnTimeUpdated?.Invoke(ElapsedTime); // Cập nhật thời gian về 0
        }

        private void Update()
        {
            if (isRunning)
            {
                ElapsedTime += Time.deltaTime;
                OnTimeUpdated?.Invoke(ElapsedTime);
            }
        }
    }
}