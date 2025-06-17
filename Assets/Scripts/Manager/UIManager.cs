using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject loseGameObjectUI;
    [SerializeField] private TMP_Text timerTMPText;
    [SerializeField] private TMP_Text recordTMPText;

    private float elapsedTime = 0;
    private bool playerWin = false;

    private string keyStringLevel1 = "recordLV1";
    private string keyStringLevel2 = "recordLV2";
    
    private void Start(){
        if(!PlayerPrefs.HasKey(keyStringLevel1))
            PlayerPrefs.SetFloat(keyStringLevel1, float.MaxValue);
        if(!PlayerPrefs.HasKey(keyStringLevel2))
            PlayerPrefs.SetFloat(keyStringLevel2, float.MaxValue);
        
        GameManager.Instance.playerLoseEventChannel.OnEventRaised += OnPlayerLose;
        GameManager.Instance.playerWinEventChannel.OnEventRaised += OnPlayerWin;
    }

    private void OnDestroy()
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

    private void Update()
    {
        if (!playerWin)
        {
            elapsedTime += Time.deltaTime;
        
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        
            timerTMPText.SetText("Time: " + minutes + ":" + seconds);
        }   
    }

    private void OnPlayerLose()
    {
        if (!loseGameObjectUI.activeSelf)
        {
            loseGameObjectUI.SetActive(true);
        }
    }

    private void OnPlayerWin()
    {
        playerWin = true;
        bool currentLevelIsOne = SceneManager.GetActiveScene().buildIndex == 2;
        if (currentLevelIsOne)
        {
            if (PlayerPrefs.GetFloat(keyStringLevel1) > elapsedTime)
            {
                PlayerPrefs.SetFloat(keyStringLevel1, elapsedTime);
            }
        }
        else
        {
            if (PlayerPrefs.GetFloat(keyStringLevel2) > elapsedTime)
            {
                PlayerPrefs.SetFloat(keyStringLevel2, elapsedTime);
            }
        }

        if (currentLevelIsOne)
        {
            int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat(keyStringLevel1) / 60f);
            int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat(keyStringLevel1) % 60f);
        
            recordTMPText.SetText("Record: " + minutes + ":" + seconds);
        }
        else
        {
            int minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat(keyStringLevel2) / 60f);
            int seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat(keyStringLevel2) % 60f);
        
            recordTMPText.SetText("Record: " + minutes + ":" + seconds);
        }
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