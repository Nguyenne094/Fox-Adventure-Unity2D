using UnityEngine;

public class HealPickup : MonoBehaviour
{
    private const uint healthRestore = 1;
    private AudioSource sound;

    private void Awake() {
        sound = GameObject.Find("Pickup Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Damageable damageable = other.GetComponent<Damageable>();

        if(damageable && damageable.Health != damageable.MaxHealth){
            damageable.Heal(healthRestore);
            sound.gameObject.transform.position = transform.position;
            sound?.Stop();
            sound?.Play();
            Destroy(gameObject);
        }
    }
}
