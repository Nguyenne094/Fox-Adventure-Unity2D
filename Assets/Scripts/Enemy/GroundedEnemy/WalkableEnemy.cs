using System;
using Nguyen.Event;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CheckDirection))]
[RequireComponent(typeof(WalkableEnemyHealth))]
public abstract class WalkableEnemy : MonoBehaviour
{
    [SerializeField] protected float speed = 8f;

    private Rigidbody2D rb;
    private CheckDirection checkDirection;
    private DetectionZone detectGround;
    private WalkableEnemyHealth enemyHealth;

    private Vector2 walkDirectionVector = Vector2.right;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection = WalkableDirection.Right;

    #region Properties
    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            if (_walkDirection != value)
            {
                walkDirectionVector = value == WalkableDirection.Right ? Vector2.right : Vector2.left;
                _walkDirection = value;
            }
        }
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkDirection = GetComponent<CheckDirection>();
        detectGround = GetComponentInChildren<DetectionZone>();
        enemyHealth = GetComponent<WalkableEnemyHealth>();
    }

    private void FixedUpdate()
    {
        FlipDirection();
        if (enemyHealth.IsAlive) Movement();
    }

    protected void FlipDirection()
    {
        if ((checkDirection.IsGrounded && checkDirection.IsOnWall) ||
            (checkDirection.IsGrounded && !detectGround.HaveGround))
        {
            WalkDirection = WalkDirection == WalkableDirection.Right
                ? WalkableDirection.Left
                : WalkableDirection.Right;
            
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * walkDirectionVector.x,
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    private void Movement()
    {
        rb.linearVelocity = enemyHealth?.IsAlive == true
            ? new Vector2(walkDirectionVector.x * speed, rb.linearVelocity.y)
            : Vector2.zero;
    }
}
