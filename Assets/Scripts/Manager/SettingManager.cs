using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    public void ReturnHome(){
        StartCoroutine(LoadHomeScene());
    }

    public void OnRestart(){
        StartCoroutine(Restart());
    }

    public void LoadNextLevel(){
        StartCoroutine(NextLevel());
    }

    private IEnumerator LoadHomeScene()
    {
        transition.SetTrigger(AnimationString.sceneTransition);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Home");
    }

    public IEnumerator Restart(){
        transition.SetTrigger(AnimationString.sceneTransition);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator NextLevel(){
        transition.SetTrigger(AnimationString.sceneTransition);

        yield return new WaitForSeconds(transitionTime);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}
