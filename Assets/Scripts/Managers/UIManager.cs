using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject loseGameObjectUI;
    [SerializeField] private TMP_Text timerTMPText;
    [SerializeField] private TMP_Text recordTMPText;

    [Header("Star System")]
    [SerializeField] private LevelTimer levelTimer;

    private bool playerWin;
    private int starsEarned;

    private string keyStringLevel1 = "recordLV1";
    private string keyStringLevel2 = "recordLV2";
    
    private StringBuilder timerSb = new StringBuilder("Time: ", 10);


    private void Start(){
        GameManager.Instance.playerLoseEventChannel.OnEventRaised += OnPlayerLose;
        GameManager.Instance.playerWinEventChannel.OnEventRaised += OnPlayerWin;

        levelTimer.OnTimeUpdated.AddListener(UpdateTimerUI);
    }

    private void OnDisable()
    {
        GameManager.Instance.playerLoseEventChannel.OnEventRaised -= OnPlayerLose;
        GameManager.Instance.playerWinEventChannel.OnEventRaised -= OnPlayerWin;
    }

    #region Heath UI

    public void CharacterTookDamage(GameObject character, string takeDameSentence)
    {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        // TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, gameCanvas.transform).
        //     GetComponent<TMP_Text>();
        // tmpText.text = takeDameSentence;
    }

    public void CharacterHealed(GameObject character, string healSentence)
    {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        // TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPos, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        // tmpText.text = healSentence;
    }

    #endregion

    private void OnPlayerLose()
    {
        if (!loseGameObjectUI.activeSelf)
        {
            loseGameObjectUI.SetActive(true);
        }
    }

    private void SaveLeaderboardToFirebase(string levelKey, float time)
    {
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var leaderboardRef = FirebaseDatabase.DefaultInstance.GetReference("Leaderboards").Child(levelKey);

        // Sử dụng Dictionary thay vì anonymous type
        var leaderboardData = new Dictionary<string, object>
        {
            { "time", time }
        };

        leaderboardRef.Child(userId).SetValueAsync(leaderboardData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Leaderboard updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update leaderboard: " + task.Exception);
            }
        });
    }

    private void OnPlayerWin()
    {
        GameManager.Instance.IsPlayerWin = true;

        string levelKey = SceneLoader.Instance.SceneGroupManager.CurrentSceneGroup.name;

        // Lưu thời gian lên Firebase
        SaveLeaderboardToFirebase(levelKey, levelTimer.GetElapsedTime());

        // Hiển thị thời gian lên UI
        recordTMPText.SetText($"Your Time: {levelTimer.GetElapsedTime():F2} seconds");

        // Cập nhật record local nếu tốt hơn
        float bestTime = PlayerPrefs.GetFloat(levelKey);
        if (levelTimer.GetElapsedTime() < bestTime)
        {
            PlayerPrefs.SetFloat(levelKey, levelTimer.GetElapsedTime());
            PlayerPrefs.Save();
            recordTMPText.SetText($"New Record: {levelTimer.GetElapsedTime():F2} seconds");
        }
        else
        {
            recordTMPText.SetText($"Your Time: {levelTimer.GetElapsedTime():F2} seconds\nBest: {bestTime:F2} seconds");
        }
    }

    private void UpdateTimerUI(float elapsedTime)
    {
        timerTMPText.SetText($"Time: {Mathf.FloorToInt(elapsedTime / 60)}:{Mathf.FloorToInt(elapsedTime % 60)}");
    }

    public void OnEscape(InputAction.CallbackContext ctx){
        if(ctx.started){
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " :" + this.GetType() + " :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
                Application.Quit();
            #elif (UNITY_WEBGL)
                SceneManager.LoadScene("QuitScene");
            #endif
        }
    }
}