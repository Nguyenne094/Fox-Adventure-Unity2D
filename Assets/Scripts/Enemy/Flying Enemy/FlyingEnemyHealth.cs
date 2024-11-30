using System;
using UnityEngine;

public class FlyingEnemyHealth : EnemyHealth
{
    public override void TakeDamage(float direction)
    {
        _animator.SetTrigger(AnimationString.hurtTrigger);
    }
}