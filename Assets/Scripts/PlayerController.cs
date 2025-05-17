using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnAttacking;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    public int maxHealth = 100;
    public int currentHealth;

    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;
    public float attackRange = 1.5f;
    public int damage = 10;

    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveActions;
    private InputAction lookActions;
    private InputAction attackActions;
    private Vector2 moveAmt;
    private Vector2 lookAmt;
    private Rigidbody rigidbody;

    [SerializeField] private PlayerAnimator playerAnimator;
    private bool isWalking;
    private bool isAttacking;
    private bool isTakeDamage;
    private bool isVictory;
    private bool isIdle;

    public bool canMove = true;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
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
            Walking(moveDirection);
            Rotating(moveDirection);
        }
    }

    private void Walking(Vector3 moveDir)
    {
        // set animator variables
        isWalking = moveAmt != Vector2.zero;


        rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotating(Vector3 moveDir)
    {
        if (isWalking)
        {
            float rotationAmount = lookAmt.y * rotateSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

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

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsTakeDamage()
    {
        return isTakeDamage;
    }

    public bool IsVictory()
    {
        return isVictory;
    }

    public void CanMove()
    {
        canMove = true;
    }

    public void TakeDamage(int amount)
    {

        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;
        // animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            Die();
        }
    }

    void Die()
    {
        // animator.SetTrigger("die");
        gameObject.SetActive(false);
    }

}
