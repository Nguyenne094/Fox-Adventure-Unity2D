using System;
using Nguyen.Event;
using Script.Player;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utils;

[RequireComponent(typeof(Rigidbody2D), typeof(CheckDirection), typeof(Animator))]
public class Player : Singleton<Player>
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private ParticleSystem _dustStep;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform projectileParent;

    private Rigidbody2D _rb;
    private Animator _animator;
    private CheckDirection _checkDirection;
    private AudioSource _jumpSound;
    private PlayerTouchController _playerTouchController;
    private PlayerHealth _playerHealth;
    private PlayerInformationPresenter _playerInformationPresenter;

    private bool _isRunning = false;
    private bool _isFacingRight = true;

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _animator.SetBool(AnimationString.isRunning, value);
            _isRunning = value;
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * (value ? 1 : -1),
                transform.localScale.y,
                transform.localScale.z);
            _isFacingRight = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _checkDirection = GetComponent<CheckDirection>();
        _dustStep = GetComponentInChildren<ParticleSystem>();
        _jumpSound = GetComponentInChildren<AudioSource>();
        _playerTouchController = GetComponent<PlayerTouchController>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerInformationPresenter = GetComponent<PlayerInformationPresenter>();
    }

    private void Start()
    {
        _playerInformationPresenter.Initialize(0, 3);
    }

    private void FixedUpdate()
    {
        if (!_playerHealth.IsAlive) return;

        IsRunning = _playerTouchController.MoveLeftButtonClicked || _playerTouchController.MoveRightButtonClicked;
        if (IsRunning)
        {
            float horizontalInput = _playerTouchController.MoveLeftButtonClicked ? -1 : (_playerTouchController.MoveRightButtonClicked ? 1 : 0);
            _rb.linearVelocity = new Vector2(horizontalInput * speed, _rb.linearVelocity.y);
            FlipDirection(horizontalInput);
        }

        if (IsRunning && _checkDirection.IsGrounded)
            _dustStep.Play();
    }

    private void LateUpdate()
    {
        _animator.SetFloat(AnimationString.yVelocity, _rb.linearVelocity.x);
    }

    public void OnJump()
    {
        if (_checkDirection.IsGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpSound?.Stop();
            _jumpSound?.Play();
            _animator.SetTrigger(AnimationString.isJumping);
        }
    }

    public void OnFire()
    {
        if (_playerInformationPresenter.HaveCherry())
        {
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation, projectileParent);
            _playerInformationPresenter.RemoveCherry();
        }
    }

    private void FlipDirection(float horizontalInput)
    {
        if (horizontalInput > 0 && !IsFacingRight) IsFacingRight = true;
        else if (horizontalInput < 0 && IsFacingRight) IsFacingRight = false;
    }
}