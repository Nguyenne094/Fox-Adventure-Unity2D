using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    public GameObject uiPanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;

    public void ShowText(string text)
    {
        uiPanel.SetActive(true);
        dialogueText.text = text;
    }

    public void SetSpeakerName(string name)
    {
        speakerNameText.text = name;
    }


    public void Hide()
    {
        uiPanel.SetActive(false);
    }
}
