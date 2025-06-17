using System.Collections;
using Manager;
using Nguyen.Event;
using UI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Damage Settings")]
    [SerializeField] private AudioClip damagedAudioClip;
    [SerializeField] private float invincibleTime = 0.25f;
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private Vector2 normalizedHitVector = new Vector2(2, 4);
    [SerializeField] private float hitForce = 1f;

    [Header("Event Channels")]
    public FloatEventChannelSO playerTakeDamageChannel;
    public IntEventChannelSO playerHealChannel;
    [SerializeField] private UnityEvent playerDie;

    private int _health;
    private bool _isInvincible;
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerInformationPresenter _playerInformationPresenter;

    public int MaxHealth { get; private set; } = 3; // Default max health
    public int Health 
    { 
        get => _health;
        set 
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            _playerInformationPresenter.UpdateHeartView(); // Update UI directly
            if (_health <= 0 && IsAlive)
            {
                playerDie.Invoke();
                GameManager.Instance.playerLoseEventChannel.RaiseEvent();
            } 
        }
    }
    public bool IsAlive { get; private set; } = true;
    public int Damage { get; set; } = 1;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInformationPresenter = GetComponent<PlayerInformationPresenter>();
    }

    private void Start()
    {
        SubscribeToEvents();
        Health = MaxHealth; // Initialize health
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        playerHealChannel.OnEventRaised += Heal;
        playerDie.AddListener(Die);
    }

    private void UnsubscribeFromEvents()
    {
        playerHealChannel.OnEventRaised -= Heal;
        playerDie.RemoveListener(Die);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAlive && !_isInvincible && other.CompareTag("Enemy"))
        {
            float direction = Mathf.Sign(other.transform.position.x - transform.position.x);
            TakeDamage(direction);
        }
    }

    public void TakeDamage(float direction)
    {
        if (!IsAlive) return; // Prevents damage after death

        //Effects
        Vector2 force = new Vector2(normalizedHitVector.x * -direction, normalizedHitVector.y).normalized * hitForce;
        _rb.linearVelocity = Vector2.zero; // Reset velocity for consistent force application.
        _rb.AddForce(force, ForceMode2D.Impulse);
        _animator.SetTrigger(AnimationString.hurtTrigger);
        AudioSource.PlayClipAtPoint(damagedAudioClip, transform.position);
        StartCoroutine(InvincibleTimer());
        StartCoroutine(GetDamagedEffect());
        
        Health -= Damage; // Logic
        
        _playerInformationPresenter.Damage(); // Update UI
    }

    public void Heal(int healAmount = 1)
    {
        Health += healAmount; // Update Logic
        _playerInformationPresenter.Heal(); // Update UI
    }

    public void Die()
    {
        IsAlive = false;
        _animator.SetBool(AnimationString.isAlive, IsAlive);
        Destroy(gameObject, destroyTime);
    }

    private IEnumerator InvincibleTimer()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        _isInvincible = false;
    }

    private IEnumerator GetDamagedEffect()
    {
        // Add your damage effect coroutine here,  e.g., screen flash
        yield return null;
    }
}