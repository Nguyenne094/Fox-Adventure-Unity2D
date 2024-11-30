using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject loseGameObjectUI;

    private void Start(){
        GameManager.Instance.playerLoseEventChannel.OnEventRaised += OnPlayerLose;
    }

    private void OnDestroy()
    {
        GameManager.Instance.playerLoseEventChannel.OnEventRaised -= OnPlayerLose;
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