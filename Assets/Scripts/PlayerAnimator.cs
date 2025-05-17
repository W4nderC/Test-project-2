using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_TAKE_DAMAGE = "isTakeDamage";
    private const string IS_VICTORY = "isVictory";
    private const string IS_WALKING = "isWalking";
    private const string IS_DEATH = "isDeath";

    [SerializeField] private PlayerController playerController;

    public Animator animator;

    // private enum PlayerState
    // {
    //     Idle,
    //     Walking,
    //     Attacking,
    //     TakeDamage,
    //     Victory,
    // }

    // private PlayerState currentState = PlayerState.Idle;

    private void Start()
    {
        playerController.OnAttacking += PlayerController_OnAttacking;
        playerController.OnDeath += PlayerController_OnDeath;
        playerController.OnTakeDamage += PlayerController_OnTakeDamage;
    }

    private void PlayerController_OnTakeDamage(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_TAKE_DAMAGE);
    }

    private void PlayerController_OnDeath(object sender, EventArgs e)
    {
        animator.SetBool(IS_DEATH, true);
    }

    private void PlayerController_OnAttacking(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_ATTACKING);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking());

    }

    public void OnAttackAnimationFinished()
    {
        playerController.canMove = true;
    }

}
