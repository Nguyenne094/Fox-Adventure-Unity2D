using UnityEngine;

public enum TextEffectType { None, Wave, Shake, FadeIn, ScaleUp }

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName; // 👈 tên người nói
        [TextArea(2, 5)] public string text;
        public AudioClip voiceClip;
        public string animationTrigger;
        public float autoNextDelay = -1f;
        public TextEffectType textEffect;
    }

    public DialogueLine[] dialogueLines;
}
