using TMPro;
using UnityEngine;

public class SystemUtilities : MonoBehaviour {
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
}