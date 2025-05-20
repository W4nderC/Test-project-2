using UnityEngine;

public interface IEnemyHumanoid
{
    public void TakeDamage(int damage);

    public void Die();
    
    public void MeleeAttack();
}
