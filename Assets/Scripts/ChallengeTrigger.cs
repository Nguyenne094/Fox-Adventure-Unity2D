using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChallengeTrigger : MonoBehaviour
{
    [Header("Challenge Data")]
    [SerializeField] private List<ChallengeData> challengeDataList;

    [Header("UI Elements")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private Text questionText;
    [SerializeField] private Button answerButton1;
    [SerializeField] private Button answerButton2;

    [Header("Events")]
    public UnityEvent onChallengeCompleted;
    public UnityEvent onChallengeFailed;

    [Header("Settings")]
    [SerializeField] private int requiredCorrectAnswers;

    [Header("Player Control Panel")]
    [SerializeField] private GameObject playerControlPanel;

    private int currentQuestionIndex = 0;
    private int correctAnswersCount = 0;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            ShowQuestionUI();
        }
    }

    private void ShowQuestionUI()
    {
        if (currentQuestionIndex >= challengeDataList.Count)
        {
            EndChallenge();
            return;
        }

        // Deactivate player control panel
        playerControlPanel.SetActive(false);

        // Activate the question panel and set the question and answers
        questionPanel.SetActive(true);
        var currentChallenge = challengeDataList[currentQuestionIndex];
        questionText.text = currentChallenge.question;
        answerButton1.GetComponentInChildren<Text>().text = currentChallenge.answers[0];
        answerButton2.GetComponentInChildren<Text>().text = currentChallenge.answers[1];

        // Add listeners to buttons
        answerButton1.onClick.AddListener(() => CheckAnswer(0));
        answerButton2.onClick.AddListener(() => CheckAnswer(1));
    }

    private void CheckAnswer(int playerAnswer)
    {
        // Remove listeners to prevent duplicate calls
        answerButton1.onClick.RemoveAllListeners();
        answerButton2.onClick.RemoveAllListeners();

        var currentChallenge = challengeDataList[currentQuestionIndex];
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
        questionPanel.SetActive(false);

        // Reactivate player control panel
        playerControlPanel.SetActive(true);

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
