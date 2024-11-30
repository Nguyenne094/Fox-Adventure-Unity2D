using System;

interface IDamageable
{
    public int Damage { get; set; }
    public int MaxHealth { get; }
    public int Health { get; }
    public bool IsAlive { get; }

    /// <summary>
    /// Just for playing take damage effect, not for logic
    /// </summary>
    public void TakeDamage(float direction);

    public void Die();
}