using UnityEngine;

/// <summary>
/// Enemy Health, Effects
/// </summary>
public class WalkableEnemyHealth : EnemyHealth
{
    private Animator animator;
    private CheckDirection checkDirection;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        checkDirection = GetComponent<CheckDirection>();
    }

    public override void TakeDamage(float direction)
    {
        if (checkDirection.IsGrounded)
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