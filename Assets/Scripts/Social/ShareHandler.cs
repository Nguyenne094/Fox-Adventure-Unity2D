using UnityEngine;
using System.Collections;
using System.IO;

public class ShareHandler : MonoBehaviour
{
    public string fileName = "screenshot.png";

    public void ShareScreenshot()
    {
        StartCoroutine(ShareCoroutine());
    }

    IEnumerator ShareCoroutine()
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
}
