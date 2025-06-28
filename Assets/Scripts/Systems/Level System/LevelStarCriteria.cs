using UnityEngine;

[CreateAssetMenu(fileName = "LevelStarCriteria", menuName = "Game/Level Star Criteria")]
public class LevelStarCriteria : ScriptableObject
{    public int timeLimitInSeconds; // Thời gian giới hạn để đạt sao
    public bool requireNoDamage; // Điều kiện không mất HP
    public int requiredStarsToCollect; // Số lượng Star cần thu thập
}
