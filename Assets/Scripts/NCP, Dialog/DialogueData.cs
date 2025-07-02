using UnityEngine;

public enum TextEffectType { None, Wave, Shake, FadeIn, ScaleUp }

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        [TextArea(2, 5)] public string text;

        [Header("Animation")]
        public string animationTrigger;

        [Header("Voice")]
        public AudioClip voiceClip;
        [Range(0, 1)] public float voiceVolume = 1f;
        public bool stopVoideOnFinish;
        public float autoNextDelay = -1f;
        
        [Header("Text Effect")]
        public TextEffectType textEffect;
    }

    public DialogueLine[] dialogueLines;
}
