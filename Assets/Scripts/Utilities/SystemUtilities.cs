using System.Collections;
using System.IO;
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

    public void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("URL is empty or null.");
        }
    }

    public void OpenURL(TMP_Text urlText)
    {
        if (urlText != null && !string.IsNullOrEmpty(urlText.text))
        {
            Application.OpenURL(urlText.text);
        }
        else
        {
            Debug.LogWarning("URL text is empty or null.");
        }
    }

    public void CaptureAndShareScreenshot()
    {
        Capture();
        ShareScreenshot(); 
    }
    
    public void Capture(string fileName = "screenshot.png")
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("Screenshot saved to: " + path);
    }

    public void ShareScreenshot()
    {
        StartCoroutine(ShareCoroutine());
    }

    private IEnumerator ShareCoroutine(string fileName = "screenshot.png")
    {
        yield return new WaitForEndOfFrame();

        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            yield break;
        }

        new NativeShare()
            .AddFile(filePath)
            .SetSubject("Check this out!")
            .SetText("I just captured this screenshot!")
            .Share();
    }
    
    public void QuitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}