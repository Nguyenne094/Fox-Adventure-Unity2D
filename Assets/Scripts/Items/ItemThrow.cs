using Manager;
using Network;
using UnityEngine;

public class ItemThrow : MonoBehaviour, IAttackable
{
    [SerializeField] private float fireSpeed = 2f;
    [SerializeField] private int damage = 1;
    
    private Rigidbody2D rb;
    private PlayerRpc player;

    public int Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        player = SceneReferenceManager.Instance.Player;
        rb.linearVelocity = new Vector2(Vector2.right.x * fireSpeed * player.transform.localScale.x, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}