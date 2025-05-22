using System;
using UnityEngine;

public class AllyAnimationController : MonoBehaviour
{
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_TAKE_DAMAGE = "isTakeDamage";
    private const string IS_WALKING = "isWalking";
    private const string IS_DEATH = "isDeath";

    [SerializeField] private AllyController allyController;

    public Animator animator;


    private void Start()
    {
        allyController.OnAttacking += AllyController_OnAttacking;
        allyController.OnDeath += AllyController_OnDeath;
        allyController.OnTakeDamage += AllyController_OnTakeDamage;
    }

    private void AllyController_OnTakeDamage(object sender, EventArgs e)
    {
        animator.SetBool(IS_TAKE_DAMAGE, true);
    }

    private void AllyController_OnDeath(object sender, EventArgs e)
    {
        allyController.isDead = true;
        animator.SetBool(IS_DEATH, true);
    }

    private void AllyController_OnAttacking(object sender, EventArgs e)
    {
        animator.SetBool(IS_ATTACKING, true);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, allyController.isWalking);
    }

    public void OnAttackAnimationFinished()
    {
        animator.SetBool(IS_ATTACKING, false);
        allyController.isWalking = true;
    }

    public void OnTakeDamageAnimationFinished()
    {
        animator.SetBool(IS_TAKE_DAMAGE, false);
        animator.SetBool(IS_ATTACKING, false);
        allyController.isTakeDamage = false;
    }

    public void OnDeathAnimationFinished()
    {
        allyController.Die();
    }
}
