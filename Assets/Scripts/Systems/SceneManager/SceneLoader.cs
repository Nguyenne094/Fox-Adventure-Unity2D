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
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroupManager _sceneGroupManager;
    public SceneGroupManager SceneGroupManager => _sceneGroupManager;

    public override void Awake()
    {
        base.Awake();
        _sceneGroupManager = GetComponent<SceneGroupManager>();
    }

    /// <summary>
    /// Loads a scene group asynchronously with progress tracking.
    /// </summary>
    /// <param name="data">The scene group data to load.</param>
    public async Task LoadSceneGroup(SceneGroupDataSO data)
    {
        float currentProgress = 0;

        if (data == null)
        {
            Debug.LogError("[Scene Manager] Error: SceneGroupData is null");
            return;
        }

        // Tracks the progress of the scene loading process.
        ProgressInformation progress = new ProgressInformation();
        progress.OnProgressChanged += target => currentProgress = Mathf.Min(target, 1);

        EnableCanvas(); // Enable the loading canvas.
        await ImplementLoadingSceneProgress(); // Display loading progress (to be implemented).
        await SceneGroupManager.LoadSceneGroup(data, progress); // Load the scene group.
        StartCoroutine(DisableCanvas(currentProgress));
    }

    /// <summary>
    /// Displays the loading progress during the scene loading process.
    /// </summary>
    private async Task ImplementLoadingSceneProgress()
    {
        await Task.Delay(0);
        // TODO: Implement loading scene progress.
    }

    /// <summary>
    /// Enables or disables the loading canvas.
    /// </summary>
    /// <param name="enable">True to enable the canvas, false to disable it.</param>
    private void EnableCanvas(bool enable = true)
    {
        _loadingCanvas.gameObject.SetActive(enable);
    }

    private IEnumerator DisableCanvas(float progress)
    {
        var animator = _loadingCanvas.GetComponent<Animator>();
        // Get current animation clip length
        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        float animLength = clips.Length > 0 ? clips[0].clip.length : 0f;
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

        animator.SetTrigger(AnimationString.Success);
    }
}