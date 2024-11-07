using System;

interface IDamageable{
    public uint MaxHealth { get; set; }
    public uint Health { get; set; }

    public void Hit(uint damage);
    public void Die();
}