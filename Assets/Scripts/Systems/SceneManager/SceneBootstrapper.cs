using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
public class SceneBootstrapper : Singleton<SceneBootstrapper>
{
    //!!! THIS ATTRIBUTE ALWAYS CALL THIS FUNCTION BEFORE SCENE LOAD 
    //EVENT THOUGH THIS SCRIPT IS ADDED IN GAME OBJECT OR NOT
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // public static void Bootstrap()
    // {
    //     Debug.Log("Scene Bootstrapper Initializing ...");
    //     SceneManager.LoadSceneAsync("Base", LoadSceneMode.Additive);
    // }
}