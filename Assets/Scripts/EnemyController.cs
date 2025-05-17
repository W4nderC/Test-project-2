using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public event EventHandler OnAttacking;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public Animator animator;
    public bool canMove = true;

    private bool isWalking;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        // animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // animator.SetTrigger("die");
        gameObject.SetActive(false);
    }

    public void MeleeAttack()
    {
        canMove = false;


        OnAttacking?.Invoke(this, EventArgs.Empty);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
