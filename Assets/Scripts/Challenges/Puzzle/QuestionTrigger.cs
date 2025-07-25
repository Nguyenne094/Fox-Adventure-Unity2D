using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionTrigger : MonoBehaviour
{
    [Header("Challenge Data")]
    [SerializeField] private List<QuestionDataSO> questionDataList;

    [Header("UI Elements")]
    [SerializeField] private RectTransform questionPanel;
    [SerializeField] private RectTransform successPanel;
    [SerializeField] private RectTransform failurePanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button answerButton1;
    [SerializeField] private Button answerButton2;

    [Header("Events")]
    public UnityEvent onChallengeStarted;
    public UnityEvent onChallengeCompleted;
    public UnityEvent onChallengeFailed;

    [Header("Settings")]
    [SerializeField] private int requiredCorrectAnswers;

    [Header("Player Control Panel")]
    [SerializeField] private GameObject playerControlPanel;

    private int currentQuestionIndex = 0;
    private int correctAnswersCount = 0;
    private bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered) return;

        if (collision.CompareTag("Player"))
        {
            isTriggered = true;
            onChallengeStarted?.Invoke();
            ShowQuestionUI();
        }
    }

    private void ShowQuestionUI()
    {
        if (currentQuestionIndex >= questionDataList.Count)
        {
            EndChallenge();
            return;
        }

        // Deactivate player control panel
        playerControlPanel.SetActive(false);

        // Activate the question panel and set the question and answers
        questionPanel.gameObject.SetActive(true);
        var currentChallenge = questionDataList[currentQuestionIndex];
        questionText.text = currentChallenge.question;

        // Ensure only two answers are handled
        if (currentChallenge.answers.Length < 2)
        {
            Debug.LogError("Challenge must have exactly 2 answers.");
            EndChallenge();
            return;
        }

        answerButton1.GetComponentInChildren<TMP_Text>().text = currentChallenge.answers[0];
        answerButton2.GetComponentInChildren<TMP_Text>().text = currentChallenge.answers[1];

        // Add listeners to buttons
        answerButton1.onClick.AddListener(() => CheckAnswer(0));
        answerButton2.onClick.AddListener(() => CheckAnswer(1));
    }

    private void CheckAnswer(int playerAnswer)
    {
        // Remove listeners to prevent duplicate calls
        answerButton1.onClick.RemoveAllListeners();
        answerButton2.onClick.RemoveAllListeners();

        var currentChallenge = questionDataList[currentQuestionIndex];
        if (playerAnswer == currentChallenge.correctAnswerIndex)
        {
            correctAnswersCount++;
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Wrong Answer!");
        }

        currentQuestionIndex++;
        ShowQuestionUI();
    }

    private void EndChallenge()
    {
        questionPanel.gameObject.SetActive(false);

        if (correctAnswersCount >= requiredCorrectAnswers)
        {
            Debug.Log("Challenge Completed!");
            onChallengeCompleted?.Invoke();
        }
        else
        {
            Debug.Log("Challenge Failed!");
            onChallengeFailed?.Invoke();
        }
    }
}
