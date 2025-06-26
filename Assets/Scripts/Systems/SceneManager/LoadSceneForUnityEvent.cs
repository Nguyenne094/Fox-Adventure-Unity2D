using UnityEngine;

public class LoadSceneForUnityEvent : MonoBehaviour {
    public void LoadScene(SceneGroupDataSO sceneGroup)
    {
        SceneLoader.Instance.LoadScene(sceneGroup);
    }
}