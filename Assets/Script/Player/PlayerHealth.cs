using System;
using System.Collections;
using Manager;
using Nguyen.Event;
using UnityEngine;
using UnityEngine.Serialization;
using Nguyen.Event;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _health;
    [SerializeField] private float invincibleTime = 0.25f;
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private Vector2 normalizedHitVector = new Vector2(2, 4);
    [SerializeField] private float hitForce = 1;

    [Space(5), Header("Event Channel SO")] 
    public FloatEventChannelSO playerTakeDamageChannel;
    public IntEventChannelSO playerHealChannel;
    public VoidEventChannelSO playerDieChannel;
    
    private int damage = 1;
    private bool _isAlive = true;
    private bool isInvincible = false;
    private const string enemyTag = "Enemy";
    
    private Animator animator;

    private Rigidbody2D rb;

    #region Properties

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Health { 
        get{
            return _health;
        } 
        set{
            _health = value;
            if(_health <= 0){
                IsAlive = false;
            }
        } 
    }
    public bool IsAlive { 
        get{
            return _isAlive;
        } 
        set{
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);
        }
    }
    public int Damage 
    { 
        get => damage;
        set => damage = value;
    }
    
    #endregion
    
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTakeDamageChannel.OnEventRaised += TakeDamage;
        playerHealChannel.OnEventRaised += Heal;
        playerDieChannel.OnEventRaised += Die;
    }

    private void OnDisable()
    {
        playerTakeDamageChannel.OnEventRaised -= TakeDamage;
        playerHealChannel.OnEventRaised -= Heal;
        playerDieChannel.OnEventRaised -= Die;
    }

    private void Start(){
        _health = _maxHealth;
    }

    private IEnumerator InvincibleTimer(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void Die()
    {
        animator.SetBool(AnimationString.isAlive, false);
        IsAlive = false;
        Destroy(gameObject, destroyTime);
    }

    /// <summary>
    /// Just for playing take damage effect, not for logic
    /// </summary>
    public void TakeDamage(float direction)
    {
        animator.SetTrigger(AnimationString.hurtTrigger);
        
        if (direction == null) direction = 1;
        direction = Mathf.Sign(direction);
        Vector2 force = new Vector2(normalizedHitVector.x * -direction, normalizedHitVector.y).normalized * hitForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(InvincibleTimer());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAlive && !isInvincible && other.CompareTag(enemyTag))
        {
            IDamageable damageable = other.GetComponent<EnemyHealth>();
            if(IsAlive && !isInvincible && damageable != null){
                Health -= damageable.Damage;
                if(Health > 0)
                {
                    float direction = (other.transform.position.x - transform.position.x);
                    playerTakeDamageChannel.RaiseEvent(direction);
                }
                else if (Health == 0)
                {
                    GameManager.Instance.playerLoseEventChannel.RaiseEvent();
                }
            }
        }
    }

    public void Heal(int healAmount){
        if(IsAlive){
            uint healthCanRestore = (uint)Mathf.Max(MaxHealth - Health, 0);
            healAmount = Mathf.Clamp(healAmount, 0, MaxHealth - Health);
            Health += healAmount;
        }
    }
}
