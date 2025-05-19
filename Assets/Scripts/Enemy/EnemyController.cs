using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemyHumanoid
{
    public event EventHandler OnAttacking;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    public int baseHealth = 100;
    public int maxHealth;
    public int currentHealth;
    public int baseDamage = 10;
    public int damage;
    public Animator animator;
    public bool canMove = true;

    public bool isWalking;
    public bool isDead;

    private void OnEnable()
    {
        maxHealth = baseHealth + (LevelGeneratorManager.currentLevel * 5);
        damage = baseDamage + (LevelGeneratorManager.currentLevel * 2);
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;

        currentHealth = maxHealth; isDead = false;
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);
        }
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);
        }
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {

    }

    public void TakeDamage(int amount)
    {
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;
        // animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Die()
    {
        // animator.SetTrigger("die");
        // gameObject.SetActive(false);

        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);

        // player win if all enemies are dead
        if (ObjectPoolManager.CheckPoolChildInactive(ObjectPoolManager.PoolType.Enemy))
        {
            GameManager.Instance.PlayerWin();
        }
    }

    public void MeleeAnimation()
    {
        OnAttacking?.Invoke(this, EventArgs.Empty);
    }


}
