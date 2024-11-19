using UnityEngine;

public class HealPickup : MonoBehaviour
{
    private const int healAmount = 1;
    private AudioSource sound;

    private void Awake() {
        sound = GameObject.Find("Pickup Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if(playerHealth && playerHealth.Health != playerHealth.MaxHealth){
            playerHealth.playerHealChannel.RaiseEvent(healAmount);
            sound.gameObject.transform.position = transform.position;
            sound?.Stop();
            sound?.Play();
            Destroy(gameObject);
        }
    }
}
