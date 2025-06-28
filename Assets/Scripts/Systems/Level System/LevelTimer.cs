using UnityEngine;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    public UnityEvent<float> OnTimeUpdated; // Sự kiện cập nhật thời gian
    public UnityEvent OnTimeLimitReached; // Sự kiện khi hết thời gian

    private float elapsedTime;
    private bool isRunning;

    public void StartTimer()
    {
        elapsedTime = 0f;
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
        elapsedTime = 0f;
        isRunning = false;
        OnTimeUpdated?.Invoke(elapsedTime); // Cập nhật thời gian về 0
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            OnTimeUpdated?.Invoke(elapsedTime);
        }
    }
}
