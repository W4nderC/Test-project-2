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
    private AttackZone attackZone;

    private void Start()
    {
        playerController.OnAttacking += PlayerController_OnAttacking;
        playerController.OnDeath += PlayerController_OnDeath;
        playerController.OnTakeDamage += PlayerController_OnTakeDamage;

        attackZone = GetComponentInChildren<AttackZone>();
    }

    private void PlayerController_OnTakeDamage(object sender, EventArgs e)
    {
        animator.SetBool(IS_TAKE_DAMAGE, true);
    }

    private void PlayerController_OnDeath(object sender, EventArgs e)
    {
        animator.SetBool(IS_DEATH, true);
    }

    private void PlayerController_OnAttacking(object sender, EventArgs e)
    {
        animator.SetBool(IS_ATTACKING, true);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking());

    }

    public void OnAttackAnimationFinished()
    {
        animator.SetBool(IS_ATTACKING, false);
        animator.SetBool(IS_TAKE_DAMAGE, false);
        playerController.canMove = true;
        playerController.isInvincible = false;

    }

    public void OnTakeDamageAnimationFinished()
    {
        animator.SetBool(IS_TAKE_DAMAGE, false);
        animator.SetBool(IS_ATTACKING, false);
        playerController.canMove = true;
        playerController.isInvincible = false;
    }

    public void OnDeathAnimationFinished()
    {
        playerController.Die();
    }

}
