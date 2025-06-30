using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;


public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData;
    public DialogueUIController uiController;
    public UnityEvent onTrigger;
    public UnityEvent onDialogueComplete;

    public Animator npcAnimator;
    public AudioSource audioSource;

    private bool hasTriggered = false;
    private int dialogueIndex = 0;
    private bool isShowingFullLine = false;
    private bool isDialogueActive = false;
    private Coroutine typeRoutine;
    private Coroutine autoNextRoutine;

    private void OnEnable()
    {
        ETouch.EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.EnhancedTouchSupport.Disable();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;
        onTrigger?.Invoke();
        StartDialogue();
    }

    private void OnFingerDown(Finger finger)
    {
        if (!isDialogueActive) return;

        if (finger.currentTouch.startScreenPosition.x < Screen.width / 2f) return;

        if (isShowingFullLine)
            NextLine();
        else
            SkipTyping();
    }

    void StartDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = true;
        PlayLine(dialogueData.dialogueLines[dialogueIndex]);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        uiController.Hide();
        onDialogueComplete?.Invoke();
    }

    void PlayLine(DialogueData.DialogueLine line)
    {
        uiController.dialogueText.GetComponent<TextEffectAnimator>().effectType = line.textEffect;
        if (typeRoutine != null) StopCoroutine(typeRoutine);
        if (autoNextRoutine != null) StopCoroutine(autoNextRoutine);

        uiController.SetSpeakerName(line.speakerName);
        typeRoutine = StartCoroutine(TypeDialogue(line));

        if (npcAnimator && !string.IsNullOrEmpty(line.animationTrigger))
            npcAnimator.SetTrigger(line.animationTrigger);

        if (audioSource && line.voiceClip)
        {
            audioSource.clip = line.voiceClip;
            audioSource.Play();
        }
    }

    void NextLine()
    {
        if (typeRoutine != null) StopCoroutine(typeRoutine);
        if (autoNextRoutine != null) StopCoroutine(autoNextRoutine);

        dialogueIndex++;
        if (dialogueIndex >= dialogueData.dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            PlayLine(dialogueData.dialogueLines[dialogueIndex]);
        }
    }

    void SkipTyping()
    {
        if (typeRoutine != null)
        {
            StopCoroutine(typeRoutine);
            DialogueData.DialogueLine line = dialogueData.dialogueLines[dialogueIndex];
            uiController.ShowText(line.text);
            isShowingFullLine = true;
        }
    }

    IEnumerator TypeDialogue(DialogueData.DialogueLine line)
    {
        isShowingFullLine = false;
        string display = "";
        uiController.ShowText("");

        foreach (char c in line.text)
        {
            display += c;
            uiController.ShowText(display);
            yield return new WaitForSeconds(0.02f);
        }

        isShowingFullLine = true;

        if (line.autoNextDelay >= 0f)
            autoNextRoutine = StartCoroutine(AutoNextAfterDelay(line.autoNextDelay));
    }

    IEnumerator AutoNextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isDialogueActive && isShowingFullLine)
        {
            NextLine();
        }
    }
}