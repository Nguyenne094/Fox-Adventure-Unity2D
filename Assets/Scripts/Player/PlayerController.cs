using Manager;
using Script.Player;
using UI;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D), typeof(DirectionChecker), typeof(Animator))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private DirectionChecker _directionChecker;
    [SerializeField] private PlayerTouchController _playerTouchController;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerPresenter _playerPresenter;

    [Header("Audio")]
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField, Range(0, 1)] private float _jumpSoundVolume = 1f;

    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private ParticleSystem _dustStep;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform projectileParent;

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

    private void Start()
    {
        _playerPresenter.Initialize(0, 3);
    }

    void Update()
    {
        if (!_playerHealth.IsAlive) return;

        IsRunning = _rb.linearVelocity.x != 0;
        
        if (IsRunning && _directionChecker.IsGrounded && !_stepAudioSource.isPlaying)
        {
            _stepAudioSource.Play();
        }
        else if (!IsRunning || !_directionChecker.IsGrounded)
        {
            _stepAudioSource.Stop();
        }

        if (IsRunning && _directionChecker.IsGrounded)
            _dustStep.Play();

    }

    private void FixedUpdate()
    {
        float horizontalInput = _playerTouchController.MoveLeftButtonClicked ? -1 : (_playerTouchController.MoveRightButtonClicked ? 1 : 0);
        _rb.linearVelocity = new Vector2(horizontalInput * speed, _rb.linearVelocity.y);
        FlipDirection(horizontalInput);
    }

    private void LateUpdate()
    {
        _animator.SetFloat(AnimationString.yVelocity, _rb.linearVelocity.y);
    }

    public void OnJump()
    {
        if (_directionChecker.IsGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            SoundManager.Instance.PlaySFX(_jumpSound, _jumpSoundVolume);
            _animator.SetTrigger(AnimationString.isJumping);
        }
    }

    public void OnFire()
    {
        if (_playerPresenter.HaveCherry())
        {
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation, projectileParent);
            _playerPresenter.RemoveCherry();
        }
    }

    private void FlipDirection(float horizontalInput)
    {
        if (horizontalInput > 0 && !IsFacingRight) IsFacingRight = true;
        else if (horizontalInput < 0 && IsFacingRight) IsFacingRight = false;
    }

    public void StopMovement()
    {
        _rb.linearVelocityX = 0;
        _playerTouchController.MoveLeftTouchExit();
        _playerTouchController.MoveRightTouchExit();
        IsRunning = false;
    }
}