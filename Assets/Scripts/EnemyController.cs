using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Animator animator;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 2f);
    }
}
