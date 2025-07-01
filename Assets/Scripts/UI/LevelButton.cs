using Firebase.Auth;
using Manager;
using Quest;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private QuestDataSO levelQuestData;
    [SerializeField] private Sprite fullStarImage;
    [SerializeField] private Sprite emptyStarImage;
    [SerializeField] private Image star1Image;
    [SerializeField] private Image star2Image;
    [SerializeField] private Image star3Image;

    private async void Start()
    {
        var db = FirebaseManager.Instance.DbRef;
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (db == null || user == null)
        {
            Debug.LogError("Firebase database reference or user is null. Cannot load quest stars.");
            return;
        }

        int stars = 0;

        try
        {
            var dataSnapshot = await db.Child("users")
                .Child(user.UserId)
                .Child("quests")
                .Child(levelQuestData.questId)
                .Child("starsEarned")
                .GetValueAsync();

            if (dataSnapshot.Exists && dataSnapshot.Value != null)
            {
                int.TryParse(dataSnapshot.Value.ToString(), out stars);
            }
            else
            {
                Debug.LogWarning($"No stars found for quest {levelQuestData.questId}. Defaulting to 0 stars.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load stars from Firebase: " + ex.Message);
        }

        SetStars(stars);
    }

    private void SetStars(int stars)
    {
        star1Image.sprite = stars >= 1 ? fullStarImage : emptyStarImage;
        star2Image.sprite = stars >= 2 ? fullStarImage : emptyStarImage;
        star3Image.sprite = stars >= 3 ? fullStarImage : emptyStarImage;

        Debug.Log($"Set stars for quest {levelQuestData.questId}: {stars}");
    }
}
