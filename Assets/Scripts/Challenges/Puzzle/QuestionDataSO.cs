using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData New", menuName = "QuestionData", order = 1)]
public class QuestionDataSO : ScriptableObject
{
    [Header("Challenge Settings")]
    public string question;
    public string[] answers;
    [Range(0, 1)] public int correctAnswerIndex;
}
