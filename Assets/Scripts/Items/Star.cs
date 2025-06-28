using Manager;
using UI;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private int starValue = 1; // Giá trị của Star

    public int StarValue => starValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.CollectStar(starValue);
                Destroy(gameObject);
            }
        }
    }
}
