using UnityEngine;

public class CherryPickup : MonoBehaviour
{
    Player player;
    public AudioSource sound;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sound = GameObject.Find("Pickup Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(player){
            player.Cherry++;
            sound.gameObject.transform.position = transform.position;
            sound?.Stop();
            sound?.Play();
            Destroy(gameObject);
        }
    }
}
