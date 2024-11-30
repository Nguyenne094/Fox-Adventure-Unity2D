using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    TextMeshProUGUI _levelText;
    public Animator transition;
    public string _level;


    private void Awake() {
        _levelText = GetComponentInChildren<TextMeshProUGUI>();
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.name = "Level " + _level;
        _levelText.text = _level.ToString();
    }

    public void StartLevel(){
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        transition.SetTrigger(AnimationString.sceneTransition);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level " + _level);
    }
}
