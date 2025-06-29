using UnityEngine;
using TMPro;
using Utils;

public class LevelController : Singleton<LevelController>
{
    [Header("References")]
    [SerializeField] private LevelStarCriteria starCriteria;
    [SerializeField] private LevelTimer levelTimer;
    [SerializeField] private TMP_Text starResultText;
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