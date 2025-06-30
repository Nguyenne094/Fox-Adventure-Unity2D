using System.IO;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    public string fileName = "screenshot.png";

    public void Capture()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if(File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Previous screenshot deleted: " + path);
        }
        ScreenCapture.CaptureScreenshot(fileName);
        Debug.Log("Screenshot saved to: " + path);
    }
}
