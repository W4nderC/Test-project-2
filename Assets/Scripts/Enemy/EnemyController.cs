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

    public float attackCooldown = 2f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    public float baseScale = 1f;         // Kích thước ở level 1
    public float scalePerLevel = 0.1f;

    private void OnEnable()
    {
        int level = LevelGeneratorManager.currentLevel;
        maxHealth = baseHealth + (level * 5);
        damage = baseDamage + (level * 2);

        //Buff
        if (level % 2 == 0)
        {
            BuffManager.Instance.buffSos[level / 2 - 1].ApplyBuffStat
            (transform, damage, maxHealth, currentHealth, atkSpd.attackCooldown,
            out damage, out maxHealth, out currentHealth, out atkSpd.attackCooldown);
        }

        currentHealth = maxHealth;
        isDead = false;

        float scaleMultiplier = baseScale + (level - 1) * scalePerLevel;
        transform.localScale = Vector3.one * scaleMultiplier;

        col.enabled = true;
        isWalking = false;
        isTakeDamage = false;
    }

    void Start()
    {
        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;

        currentHealth = maxHealth;
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
        isTakeDamage = true;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;
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
    }

}
