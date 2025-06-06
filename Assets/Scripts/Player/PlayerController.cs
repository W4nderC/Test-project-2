using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAllyHumanoid
{
    public event EventHandler OnAttacking;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    public int baseHealth = 100;
    public int maxHealth;
    public int currentHealth;
    public int baseDamage = 50;
    public int damage;

    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;
    public float attackRange = 1.5f;

    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveActions;
    private InputAction lookActions;

    private Vector2 moveAmt;
    private Vector2 lookAmt;
    private Rigidbody rigidbody;

    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = .5f;

    [SerializeField] private PlayerAnimator playerAnimator;
    public bool isWalking;
    public bool canMove = true;
    public bool isInvincible = false;
    public bool isAttacking;

    [SerializeField] private HealthBar healthBar;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();

        maxHealth = baseHealth + (LevelGeneratorManager.currentLevel * 5);
        damage = baseDamage + (LevelGeneratorManager.currentLevel * 2);
        currentHealth = maxHealth;

        canMove = true;
        isInvincible = false;

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        moveActions = InputSystem.actions.FindAction("Move");
        lookActions = InputSystem.actions.FindAction("Look");
        rigidbody = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        GameManager.Instance.OnGameWaitingToStart += GameManager_OnGameWaitingToStart;
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
        }
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
        }
    }

    private void GameManager_OnGameWaitingToStart(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
        }
    }

    void Update()
    {
        moveAmt = moveActions.ReadValue<Vector2>();
        lookAmt = lookActions.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveAmt.x, 0, moveAmt.y);
        if (canMove)
        {
            canMove = true;
            Walking(moveDirection);
            Rotating(moveDirection);            
        }
    }

    private void Walking(Vector3 moveDir)
    {
        // set animator variables
        isWalking = moveAmt != Vector2.zero;
        rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);

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

    private void Rotating(Vector3 moveDir)
    {
        if (isWalking)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

    }

    public void MeleeAttack()
    {

        // isInvincible = true;
        canMove = false;
        isWalking = false;

        OnAttacking?.Invoke(this, EventArgs.Empty);
        
        // play sound
        SoundManager.Instance.PlaySound(SoundType.PUNCH, transform);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void TakeDamage(int amount)
    {
        // player take no damge while attacking or invincible
        if (isInvincible) return;
        
        canMove = false;

        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;

        // update health bar
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        
        // give player invincibility for a short time after taking damage
        isInvincible = true;

        // play sound
        SoundManager.Instance.PlaySound(SoundType.HURT, transform);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);

        }
    }

    public void Die()
    {
        // animator.SetTrigger("die");
        // gameObject.SetActive(false);
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
        GameManager.Instance.PlayerLose();
    }

}
