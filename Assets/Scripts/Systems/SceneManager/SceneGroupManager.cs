using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nguyen.Event;

public class SceneGroupManager : MonoBehaviour
{
    public VoidEventChannelSO OnSceneGroupLoaded;
    public VoidEventChannelSO OnSceneGroupUnloaded;
    public SceneGroupDataSO CurrentSceneGroup;
    public bool SceneGroupLoaded { get; private set; }

    public async Task LoadSceneGroup(SceneGroupDataSO loadingSceneGroup, IProgress<float> progress)
    {
        SceneGroupLoaded = false;
        
        if(loadingSceneGroup.Scenes.Count <= 0 || loadingSceneGroup.Scenes == null)
        {
            Debug.LogError("[Scene Manager] Error: No scenes to load in the SceneGroupDataSO.");
            return;
        }

        await UnloadSceneGroup(loadingSceneGroup);

        var loadedScene = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScene.Add(SceneManager.GetSceneAt(i).name);
        }

        var operationGroup = new OperationGroup(loadingSceneGroup.Scenes.Count);

        foreach (var scene in loadingSceneGroup.Scenes)
        {
            if (loadedScene.Contains(scene.SceneName)) continue;

            var asyncOperation = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);

            operationGroup.Operations.Add(asyncOperation);

            asyncOperation.completed += (operation) =>
            {
                operationGroup.Operations.Remove(asyncOperation);
            };
        }

        while (!operationGroup.IsDone())
        {
            progress?.Report(operationGroup.Progress());
            await Task.Yield();
        }

        var activeScene = SceneManager.GetSceneByName(loadingSceneGroup.GetSceneByType(SceneType.Active).SceneName);
        if (activeScene.IsValid())
        {
            SceneManager.SetActiveScene(activeScene);
        }

        if (operationGroup.IsDone())
        {
            OnSceneGroupLoaded?.RaiseEvent();
            SceneGroupLoaded = true;
            CurrentSceneGroup = loadingSceneGroup;
        }

        Debug.Log($"{loadingSceneGroup} is Loaded");
    }

    public async Task UnloadSceneGroup(SceneGroupDataSO groupSceneToLoad)
    {
        if (groupSceneToLoad == null)
        {
            Debug.LogError("[Scene Manager] Error: groupSceneToLoad is null.");
            return;
        }

        var sceneCount = SceneManager.sceneCount;
        var operationGroup = new OperationGroup(sceneCount);

        for (int i = 0; i < sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var sceneData = groupSceneToLoad.GetSceneByName(scene.name);
            
            if (sceneData != null) continue;

            var asyncOperation = SceneManager.UnloadSceneAsync(scene);
            operationGroup.Operations.Add(asyncOperation);

            asyncOperation.completed += (operation) =>
            {
                operationGroup.Operations.Remove(asyncOperation);
            };
        }

        while (!operationGroup.IsDone())
        {
            await Task.Yield();
        }

        if (operationGroup.IsDone())
        {
            OnSceneGroupUnloaded?.RaiseEvent();
        }

        Debug.Log("Current scene group is Unloaded");
    }
}

public readonly struct OperationGroup
{
    public readonly HashSet<AsyncOperation> Operations;

    public OperationGroup(int size)
    {
        Operations = new HashSet<AsyncOperation>(size);
    }
    
    public bool IsDone() => Operations.Count == 0 || Operations.All(x => x.isDone);
    public float Progress() => Operations.Count == 0 ? 1 : Operations.Average(x => x.progress);
}