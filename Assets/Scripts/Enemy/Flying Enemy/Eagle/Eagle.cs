using System;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] private bool isFacingLeft = true;
    [SerializeField] private float spawnRange;

    private Vector3 spawnPosition;
    
    private DetectedZone detectedZone;
    private EnemyHealth enemyHealth;
    private CircleCollider2D col;
    private Animator animator;
    
    public bool IsFacingLeft
    {
        get { return isFacingLeft; }
        set
        {
            if (isFacingLeft != value)
            {
                transform.localScale *= new Vector2(-1, 1); // Flip along X axis
            }
            isFacingLeft = value;
        }
    }

    [Header("Setting")]
    [SerializeField] private float _speed = 0.1f;

    private void Awake()
    {
        detectedZone = GetComponentInChildren<DetectedZone>();
        enemyHealth = GetComponent<EnemyHealth>();
        col = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (enemyHealth.IsAlive)
        {
            if (!detectedZone.wasDetected)
            {
                IsFacingLeft = FacingTarget(spawnPosition);
                animator.SetBool(AnimationString.attack, false);
                MovementToTarget(spawnPosition, _speed);
            }
            if (detectedZone.wasDetected)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        IsFacingLeft = FacingTarget(Player.Instance.transform.position);
        animator.SetBool(AnimationString.attack, true);
        MovementToTarget(Player.Instance.transform.position, _speed);
    }

    private void MovementToTarget(Vector2 targetPosition, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
    }

    private bool FacingTarget(Vector3 targetPosition)
    {
        return transform.position.magnitude > targetPosition.magnitude;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectedZone.wasDetected)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, Player.Instance.transform.position);
        }

        Gizmos.color = Color.white;
        Gizmos.DrawCube(spawnPosition, new Vector3(0.1f, 0.1f, 0.1f));
    }
}