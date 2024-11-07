using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float fireSpeed = 2f;
    Player player;

    private uint damage = 1;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start() {
        rb.linearVelocity = new Vector2(Vector2.right.x * fireSpeed * player.transform.localScale.x, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Damageable damageable = other.GetComponent<Damageable>();

        if(damageable && other.CompareTag("Enemy")){
            damageable.Hit(damage);
        }
        Destroy(gameObject);
    }
}