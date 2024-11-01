using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Rigidbody2D playerRb;
    private Canvas gameCanvas;
    private Player player;
    private Damageable playerHealth;
    public TMP_Text cherryCount;
    private string cherryCountText = "0";
    public TMP_Text heartCount;
    private string heartCountText = "3";

    private void Awake()
    {
        gameCanvas = GameObject.FindObjectOfType<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }

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
        CharacterEvent.characterDamaged += CharacterTookDamage;
        CharacterEvent.characterHealed += CharacterHealed;
    }

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

    private void FixedUpdate() {
        cherryCount.text = player.cherry.ToString();
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