using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Player player;
    [SerializeField] private Damageable playerHealth;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject healthTextPrefab;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    
    [Space(5)]
    public TMP_Text cherryCount;
    private string cherryCountText = "0";
    public TMP_Text heartCount;
    private string heartCountText = "3";

    private void Start(){
        cherryCount.text = cherryCountText;
        heartCount.text = heartCountText;
    }

    private void OnEnable()
    {
        CharacterEvent.characterDamaged += CharacterTookDamage;
        CharacterEvent.characterHealed += CharacterHealed;
    }

    private void OnDisable() {
        CharacterEvent.characterDamaged -= CharacterTookDamage;
        CharacterEvent.characterHealed -= CharacterHealed;
    }

    #region Heath UI

    public void CharacterTookDamage(GameObject character, string takeDameSentence)
    {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, gameCanvas.transform).
            GetComponent<TMP_Text>();

        tmpText.text = takeDameSentence;
    }

    public void CharacterHealed(GameObject character, string healSentence)
    {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPos, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = healSentence;
    }
    

    #endregion

    #region Control UI

    public void MoveLeft()
    {
        
    }

    #endregion
    
    private void FixedUpdate() {
        cherryCount.text = player.Cherry.ToString();
        heartCount.text = playerHealth.Health.ToString();
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

    public void PauseGame(){
        Time.timeScale = 0;
    }
}