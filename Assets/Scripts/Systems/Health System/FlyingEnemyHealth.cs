using System;
using UnityEngine;

public class FlyingEnemyHealth : EnemyHealth
{
    public override void TakeDamage(float direction)
    {
        _animator.SetTrigger(AnimationString.hurtTrigger);
    }

    public override void Die()
    {
        rb.linearVelocity = Vector2.zero;
        IsAlive = false;
        _animator.SetBool(AnimationString.isAlive, IsAlive);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float currentAnimationClipLength = stateInfo.length;
        Destroy(gameObject, currentAnimationClipLength);
    }
}