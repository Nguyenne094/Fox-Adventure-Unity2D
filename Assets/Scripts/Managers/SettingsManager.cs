using UnityEngine;
using UnityEngine.UI;
using Utils;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Settings On Start")]
    [SerializeField] private bool enableVsync = true;
    [SerializeField] private int targetFrameRate = 60;

    private void Start()
    {
        QualitySettings.vSyncCount = enableVsync ? 1 : 0;
        Application.targetFrameRate = enableVsync ? targetFrameRate : -1; // -1 means no target frame rate
    }
    
    public void SetVSync(Toggle toggle)
    {
        bool enable = toggle.isOn;
        QualitySettings.vSyncCount = enable ? 1 : 0;
    }
}