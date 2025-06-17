using System;
using UnityEngine;
using Nguyen.Event;
using UnityEngine.Serialization;

public abstract class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private LayerMask hurtedByLayer;
    [SerializeField] protected float hitForce = 1;
    
    protected int _damage = 1;
    protected bool _isAlive = true;
    protected Vector2 knockBackDirection = new Vector2(2, 4);

    protected Rigidbody2D rb;
    protected Animator _animator;

    #region Properties

    public int Damage { get => _damage; set => _damage = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Health { get => _currentHealth; set => _currentHealth = value; }
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            _animator.SetBool(AnimationString.isAlive, value);
        }
    }

    #endregion

    [Space(5), Header("Event Channel SO")] 
    public Action<float> enemyTakeDamageChannel;
    public Action enemyDieChannel;
    
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
    public abstract void TakeDamage(float damage);

    public abstract void Die();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsHurtedByLayer(other.gameObject.layer))
        {
            IAttackable attackable = other.GetComponent<IAttackable>();
            if(IsAlive && attackable != null){
                Health -= attackable.Damage;
                if(Health > 0)
                {
                    float direction = (other.transform.position.x - transform.position.x);
                    enemyTakeDamageChannel?.Invoke(direction);
                }
                else if (Health <= 0)
                {
                    enemyDieChannel?.Invoke();
                    Health = 0; //Avoid health is negative number
                }
            }
        }
    }

    protected bool IsHurtedByLayer(LayerMask layer)
    {
        return ((1 << layer) & hurtedByLayer.value) != 0;
    }
}