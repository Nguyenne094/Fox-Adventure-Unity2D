using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private float transitionTime = 1f;

    public void OnStart(){
        StartCoroutine(LoadScene());
    }

    public void OnLevelSelect(){
        StartCoroutine(LoadLevelSelectScene());
    }

    private IEnumerator LoadScene()
    {
        transition.SetTrigger(AnimationString.sceneTransition);

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Level " + 1);
    }

    private IEnumerator LoadLevelSelectScene()
    {
        transition.SetTrigger(AnimationString.sceneTransition);

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(11);
    }

    public void OnExit() {
        Application.Quit();
    }
}
