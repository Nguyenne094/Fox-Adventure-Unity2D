using TMPro;
using UnityEngine;

public class SystemUtilities : MonoBehaviour
{
    public void CopyToClipboard(TMP_Text TextToCopy)
    {
        if (!string.IsNullOrEmpty(TextToCopy.text))
        {
            GUIUtility.systemCopyBuffer = TextToCopy.text;
        }
        else
        {
            Debug.LogWarning("Text to copy is empty or null.");
        }
    }

    public void CopyToClipboard(string textToCopy)
    {
        if (!string.IsNullOrEmpty(textToCopy))
        {
            GUIUtility.systemCopyBuffer = textToCopy;
        }
        else
        {
            Debug.LogWarning("Text to copy is empty or null.");
        }
    }
    
    public void QuitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}