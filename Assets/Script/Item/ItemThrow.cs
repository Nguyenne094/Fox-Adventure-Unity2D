using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    [SerializeField] private float fireSpeed = 2f;
    
    private Player player;
    private Rigidbody2D rb;
    private byte damage = 1;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start() {
        rb.linearVelocity = new Vector2(Vector2.right.x * fireSpeed * player.transform.localScale.x, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if(playerHealth){
            playerHealth.TakeDamage(damage);
        }

        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }
}