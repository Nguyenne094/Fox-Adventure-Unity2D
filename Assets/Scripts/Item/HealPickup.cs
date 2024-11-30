using Manager;
using UnityEngine;
using UI;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [Range(0,1), SerializeField] private float volume;
    
    private const string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(playerTag))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth.Health < playerHealth.MaxHealth)
            {
                playerHealth.Heal();
                SoundManager.Instance.PlaySFX(clip, volume);
                Destroy(gameObject);
            }
        }
    }
}
