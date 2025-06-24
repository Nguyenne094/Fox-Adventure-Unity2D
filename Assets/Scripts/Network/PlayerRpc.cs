using Manager;
using Script.Player;
using UI;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Network
{
    public class PlayerRpc : NetworkSingleton<PlayerRpc>
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private DirectionChecker _directionChecker;
        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private PlayerTouchController _playerTouchController;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerPresenter _playerPresenter;
        
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
        
        private SceneReferenceManager _sceneReferenceManager;

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


        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            _sceneReferenceManager = SceneReferenceManager.Instance;
            _sceneReferenceManager.SetupPlayerReference(this);
            _playerPresenter.Initialize(0, 3);//I call this function here because I want to prevent null ref error when init
        }

        private void FixedUpdate()
        {
            if (!_playerHealth.IsAlive && !IsOwner) return;

            IsRunning = _playerTouchController.MoveLeftButtonClicked || _playerTouchController.MoveRightButtonClicked;
            if (IsRunning)
            {
                float horizontalInput = _playerTouchController.MoveLeftButtonClicked ? -1 : (_playerTouchController.MoveRightButtonClicked ? 1 : 0);
                _rb.linearVelocity = new Vector2(horizontalInput * speed, _rb.linearVelocity.y);
                FlipDirection(horizontalInput);
            }

            if (IsRunning && _directionChecker.IsGrounded)
                _dustStep.Play();
        }

        private void LateUpdate()
        {
            if (!IsOwner) return;
            _animator.SetFloat(AnimationString.yVelocity, _rb.linearVelocity.y);
        }

        public void OnJump()
        {
            Debug.Log("jump");
            if (!IsOwner) return;
            Debug.Log("jump");
            if (_directionChecker.IsGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _jumpSound?.Stop();
                _jumpSound?.Play();
                _animator.SetTrigger(AnimationString.isJumping);
            }
        }

        public void OnFire()
        {
            if (!IsOwner) return;
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
    }
}