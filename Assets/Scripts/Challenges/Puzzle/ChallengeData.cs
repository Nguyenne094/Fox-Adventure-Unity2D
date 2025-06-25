using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeData", menuName = "ScriptableObjects/ChallengeData", order = 1)]
public class ChallengeData : ScriptableObject
{
    [Header("Challenge Settings")]
    public string question;
    public string[] answers;
    [Range(0, 1)] public int correctAnswerIndex;
}
