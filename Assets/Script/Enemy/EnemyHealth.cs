using System;
using UnityEngine;
using Nguyen.Event;
using UnityEngine.Serialization;


public abstract class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask hurtedByLayer;
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private Vector2 normalizedHitVector = new Vector2(2, 4);
    [SerializeField] private float hitForce = 1;

    private Rigidbody2D rb;

    #region Properties

    public int Damage { get => _damage; set => _damage = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Health { get => _currentHealth; set => _currentHealth = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    #endregion

    [Space(5), Header("Event Channel SO")] 
    public Action<float> enemyTakeDamageChannel;
    public Action enemyDieChannel;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyTakeDamageChannel += TakeDamage;
        enemyDieChannel += Die;
    }

    private void OnDisable()
    {
        enemyTakeDamageChannel -= TakeDamage;
        enemyDieChannel -= Die;
    }

    private void Start()
    {
        Health = MaxHealth;
    }
    
    /// <summary>
    /// Just for playing take damage effect, not for logic
    /// </summary>
    public virtual void TakeDamage(float direction)
    {
        if (direction == null) direction = 1;
        direction = Mathf.Sign(direction);
        Vector2 force = new Vector2(normalizedHitVector.x * -direction, normalizedHitVector.y).normalized * hitForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Die()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & hurtedByLayer.value) != 0)
        {
            IDamageable damageable = other.GetComponent<PlayerHealth>();
            if(IsAlive && damageable != null){
                Health -= damageable.Damage;
                if(Health > 0)
                {
                    float direction = (other.transform.position.x - transform.position.x);
                    enemyTakeDamageChannel.Invoke(direction);
                }
                else if (Health == 0)
                {
                    enemyDieChannel.Invoke();
                }
            }
        }
    }
}