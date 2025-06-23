using UnityEngine;

/// <summary>
/// Enemy Health, Effects
/// </summary>
public class WalkableEnemyHealth : EnemyHealth
{
    private Animator animator;
    private DirectionChecker _directionChecker;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        _directionChecker = GetComponent<DirectionChecker>();
    }

    public override void TakeDamage(float direction)
    {
        if (_directionChecker.IsGrounded)
        {
            direction = Mathf.Sign(direction);
            Vector2 force = new Vector2(knockBackDirection.x * -direction, knockBackDirection.y).normalized * hitForce;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public override void Die()
    {
        rb.linearVelocity = Vector2.zero;
        IsAlive = false;
        animator.SetBool(AnimationString.isAlive, IsAlive);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentAnimationClipLength = stateInfo.length;
        Destroy(gameObject, currentAnimationClipLength);
    }
}