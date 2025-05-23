using System;
using UnityEngine;

public class AllyController : MonoBehaviour, IAllyHumanoid
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
    public bool isAttacking;
    public bool isTakeDamage;

    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = .5f;

    [SerializeField] private Collider col;
    public float attackCooldown = 2f; // cooldown time between attacks

    [SerializeField] private HealthBar healthBar;

    private void OnEnable()
    {
        maxHealth = baseHealth + (LevelGeneratorManager.currentLevel * 5);
        damage = baseDamage + (LevelGeneratorManager.currentLevel * 2);
        currentHealth = maxHealth;
        isDead = false;

        col.enabled = true;
        isWalking = false;
        isTakeDamage = false;

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    void Start()
    {
        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        GameManager.Instance.OnGameWaitingToStart += GameManager_OnGameWaitingToStart;

        currentHealth = maxHealth; 
    }

    private void Update()
    {
        if (isDead) return;

        PlayFootstepSound();
    }

    private void PlayFootstepSound()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            footstepTimer = footstepTimerMax;

            if (isWalking)
            {
                // play sound
                SoundManager.Instance.PlaySound(SoundType.FOOTSTEPS, transform);
            }
        }
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Ally);
        }
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Ally);
        }
    }

    private void GameManager_OnGameWaitingToStart(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Ally);
        }
    }

    public void TakeDamage(int amount)
    {
        isTakeDamage = true;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;

        // update health bar
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        
        // play sound
        SoundManager.Instance.PlaySound(SoundType.HURT, transform);

        // animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            col.enabled = false;
        }
    }

    public void Die()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Ally);
    }

    public void MeleeAttack()
    {
        OnAttacking?.Invoke(this, EventArgs.Empty);

        // play sound
        SoundManager.Instance.PlaySound(SoundType.PUNCH, transform);
    }

}
