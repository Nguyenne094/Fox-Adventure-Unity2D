using System;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private bool isFacingLeft = true;
    [SerializeField] private float spawnRange;

    private Vector3 spawnPosition;
    
    private DetectedZone detectedZone;
    private EnemyHealth enemyHealth;
    private CircleCollider2D col;
    private Animator animator;
    private GameObject player;
    
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

    private void Awake()
    {
        detectedZone = GetComponentInChildren<DetectedZone>();
        enemyHealth = GetComponent<EnemyHealth>();
        col = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        player = PlayerController.Instance.gameObject;
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
            else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        IsFacingLeft = FacingTarget(player.transform.position);
        animator.SetBool(AnimationString.attack, true);
        MovementToTarget(player.transform.position, _speed);
    }

    private void MovementToTarget(Vector2 targetPosition, float speed)
    {
        transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, targetPosition.x, speed * Time.deltaTime),
            Mathf.MoveTowards(transform.position.y, targetPosition.y, speed * Time.deltaTime),
            transform.position.z
        );
    }

    private bool FacingTarget(Vector3 targetPosition)
    {
        return transform.position.magnitude > targetPosition.magnitude;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(spawnPosition, new Vector3(0.1f, 0.1f, 0.1f));
    }
}
