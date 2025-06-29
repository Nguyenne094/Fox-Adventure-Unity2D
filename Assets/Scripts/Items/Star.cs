using Manager;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private int starValue = 1; // Giá trị của Star

    public int StarValue => starValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelController levelController = LevelController.Instance;
            if (levelController != null)
            {
                levelController.CollectStar(starValue);
                Destroy(gameObject);
            }
        }
    }
}
