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

    [SerializeField] private Collider col;
    [SerializeField] private EnemyAttackZone atkSpd;

    public bool isWalking;
    public bool isDead;
    public bool isAttacking;
    public bool isTakeDamage;

    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = .5f;

    public float attackCooldown = 2f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    public float baseScale = 1f;         // Kích thước ở level 1
    public float scalePerLevel = 0.1f;

    // health recover overtime
    public float hpRecoverCooldown = 2f; // cooldown time between attacks
    private float lastHPRecoverTime = -Mathf.Infinity;

    private bool spawnBuffVisual;

    [SerializeField] private HealthBar healthBar;

    private void OnEnable()
    {
        int level = LevelGeneratorManager.currentLevel;
        maxHealth = baseHealth + (level * 5);
        damage = baseDamage + (level * 2);

        
        //Buff
        if (level > 1)
        {
            AllpyBuffStat(level, 2, 0);
        }
        if (level > 3)
        {
            AllpyBuffStat(level, 4, 1);
        }
        if (level > 5)
        {
            AllpyBuffStat(level, 6, 2);
        }
        if (level > 7)
        {
            hpRecoverCooldown = 5 - BuffManager.Instance.buffSos[3].healOverTime; // Adjust cooldown based on buff
        }

        currentHealth = maxHealth;
        isDead = false;

        float scaleMultiplier = baseScale + (level - 1) * scalePerLevel;
        transform.localScale = Vector3.one * scaleMultiplier;

        col.enabled = true;
        isWalking = false;
        isTakeDamage = false;

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    private void AllpyBuffStat(int currentLevel, int levelToSpawn, int buffIndex)
    {
            spawnBuffVisual = true;
            if (currentLevel != levelToSpawn)
            {
                spawnBuffVisual = false;
            }

            BuffManager.Instance.buffSos[buffIndex].ApplyBuffStat
            (transform, damage, maxHealth, currentHealth, atkSpd.attackCooldown,
            out damage, out maxHealth, out currentHealth, out atkSpd.attackCooldown, spawnBuffVisual);
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

        if (Time.time < lastHPRecoverTime + hpRecoverCooldown) return;
        currentHealth += 10;
        lastHPRecoverTime = Time.time;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
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

    private void GameManager_OnGameWaitingToStart(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);
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
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);

        // player win if all enemies are dead
        if (ObjectPoolManager.CheckPoolChildInactive(ObjectPoolManager.PoolType.Enemy))
        {
            GameManager.Instance.PlayerWin();
        }
    }

    public void MeleeAttack()
    {
        OnAttacking?.Invoke(this, EventArgs.Empty);

        // play sound
        SoundManager.Instance.PlaySound(SoundType.PUNCH, transform);
    }

}
