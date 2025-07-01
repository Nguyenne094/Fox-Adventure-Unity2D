using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debbuger : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private Toggle _enableVsyncToggle;
    [SerializeField] private Toggle _enableDebugModeToggle;
    [SerializeField] private GameObject _debugPanel;

    void OnEnable()
    {
        _enableVsyncToggle.onValueChanged.AddListener(OnVsyncToggleChanged);
        _enableDebugModeToggle.onValueChanged.AddListener(OnDebugModeToggleChanged);
    }

    private void OnVsyncToggleChanged(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        Application.targetFrameRate = isOn ? 60 : -1; // -1 means no target frame rate
    }

    private void OnDebugModeToggleChanged(bool isOn)
    {
        Debug.unityLogger.logEnabled = isOn;
        if (isOn)
        {
            _debugPanel.SetActive(true);
        }
        else
        {
            _debugPanel.SetActive(false);
        }
    }

    void Update()
    {
        _fpsText.text = $"FPS: {Mathf.RoundToInt(1f / Time.unscaledDeltaTime)}";
    }
}