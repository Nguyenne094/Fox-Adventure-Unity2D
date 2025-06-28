using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

/// <summary>
/// Handles the loading of scene groups with progress tracking and a loading canvas.
/// </summary>
[RequireComponent(typeof(SceneGroupManager))]
public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private SceneGroupDataSO _startSceneGroupData;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroupManager _sceneGroupManager;
    public SceneGroupManager SceneGroupManager => _sceneGroupManager;

    private Animator _animator;

    public override void Awake()
    {
        base.Awake();
        _animator = _loadingCanvas.GetComponent<Animator>();
        _sceneGroupManager = GetComponent<SceneGroupManager>();
        if(!_startSceneGroupData)
            throw new NullReferenceException("_startSceneGroupData is null. Please fix the error for running correctly.");
        LoadSceneWithoutTransition(_startSceneGroupData);
    }

    public async void LoadSceneWithoutTransition(SceneGroupDataSO sceneGroup)
    {
        try
        {
            await SceneGroupManager.LoadSceneGroup(sceneGroup, new ProgressInformation());
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load scene group: " + ex);
            throw;
        }
        await Task.Yield();
    }
    
    public async void LoadScene(SceneGroupDataSO sceneGroup)
    {
        float currentProgress = 0;

        // Tracks the progress of the scene loading process.
        ProgressInformation progress = new ProgressInformation();
        progress.OnProgressChanged += target => currentProgress = Mathf.Min(target, 1);

        await EnableCanvas(); // Enable the loading canvas.
        await ImplementLoadingSceneProgress(); // Display loading progress (to be implemented).
        try
        {
            await SceneGroupManager.LoadSceneGroup(sceneGroup, progress);
            
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load scene group: " + ex);
            throw;
        }
        StartCoroutine(DisableCanvas(currentProgress));
    }

    /// <summary>
    /// Displays the loading progress during the scene loading process.
    /// </summary>
    private async Task ImplementLoadingSceneProgress()
    {
        //TODO: Implement the logic to display loading progress.
        // This could involve updating a UI element with the current progress value.
        await Task.Delay(0);
    }

    /// <summary>
    /// Enables or disables the loading canvas.
    /// </summary>
    /// <param name="enable">True to enable the canvas, false to disable it.</param>
    private async Task EnableCanvas(bool enable = true)
    {
        _loadingCanvas.gameObject.SetActive(enable);
        // Wait for ending loading transition or loading all scenes successfully
        AnimatorClipInfo[] clips = _animator.GetCurrentAnimatorClipInfo(0);
        float animLength = clips[0].clip.length;
        int animLengthInMilliseconds = (int)(animLength * 1000);
        await Task.Delay(animLengthInMilliseconds);
    }

    private IEnumerator DisableCanvas(float progress)
    {
        // Wait for ending loading transition or loading all scenes successfully
        AnimatorClipInfo[] clips = _animator.GetCurrentAnimatorClipInfo(0);
        float animLength = clips[0].clip ? clips[0].clip.length : 0f;
        // Wait for the longer of animation or scene loading
        float timer = 0f;
        while (timer < animLength || progress < 1)
        {
            timer += Time.deltaTime;
            if (timer >= animLength && progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }

        _animator.SetTrigger(AnimationString.Success);
    }
}