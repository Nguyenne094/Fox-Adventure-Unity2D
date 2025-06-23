using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nguyen.Event;

public class SceneGroupManager : MonoBehaviour
{
    public event Action<string> OnSceneLoaded = delegate {};
    public event Action<string> OnSceneUnloaded = delegate {};
    public VoidEventChannelSO OnSceneGroupLoaded;
    public VoidEventChannelSO OnSceneGroupUnloaded;
    public SceneGroupDataSO CurrentSceneGroup;
    public bool SceneGroupLoaded { get; private set; }

    private void Awake()
    {
        OnSceneGroupLoaded.OnEventRaised += () => Debug.Log("[Scene Manager] Scene Group Loaded");
    }

    private void OnDestroy()
    {
        OnSceneGroupLoaded.OnEventRaised -= () => Debug.Log("[Scene Manager] Scene Group Loaded");
    }

    public async Task LoadSceneGroup(SceneGroupDataSO group, IProgress<float> progress)
    {
        SceneGroupLoaded = false;
        
        if(group.Scenes.Count <= 0)
        {
            Debug.LogError("[Scene Manager] Error: No scenes to load in the SceneGroupDataSO.");
            return;
        }
            
        await UnloadSceneGroup(group);

        var loadedScene = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScene.Add(SceneManager.GetSceneAt(i).name);
        }

        var operationGroup = new OperationGroup(group.Scenes.Count);

        foreach (var scene in group.Scenes)
        {
            if (loadedScene.Contains(scene.SceneName)) continue;

            var asyncOperation = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);

            operationGroup.Operations.Add(asyncOperation);

            asyncOperation.completed += (operation) =>
            {
                OnSceneLoaded?.Invoke(scene.SceneName);
                operationGroup.Operations.Remove(asyncOperation);
            };
        }

        while (!operationGroup.IsDone())
        {
            progress?.Report(operationGroup.Progress());
            await Task.Yield();
        }

        var activeScene = SceneManager.GetSceneByName(group.GetSceneByType(SceneType.Active).SceneName);
        if (activeScene.IsValid())
        {
            SceneManager.SetActiveScene(activeScene);
        }

        if (operationGroup.IsDone())
        {
            OnSceneGroupLoaded?.RaiseEvent();
            SceneGroupLoaded = true;
            CurrentSceneGroup = group;
        }
        
    }

    public async Task UnloadSceneGroup(SceneGroupDataSO groupSceneToLoad)
    {
        var sceneCount = SceneManager.sceneCount;
        var operationGroup = new OperationGroup(sceneCount);

        for (int i = 0; i < sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded || groupSceneToLoad.GetSceneByName(scene.name) != null) continue;

            var asyncOperation = SceneManager.UnloadSceneAsync(scene);
            operationGroup.Operations.Add(asyncOperation);

            asyncOperation.completed += (operation) =>
            {
                OnSceneUnloaded?.Invoke(scene.name);
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