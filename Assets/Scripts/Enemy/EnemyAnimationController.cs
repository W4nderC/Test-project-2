using System;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_TAKE_DAMAGE = "isTakeDamage";
    private const string IS_WALKING = "isWalking";
    private const string IS_DEATH = "isDeath";

    [SerializeField] private EnemyController enemyController;

    public Animator animator;


    private void Start()
    {
        enemyController.OnAttacking += EnemyController_OnAttacking;
        enemyController.OnDeath += EnemyController_OnDeath;
        enemyController.OnTakeDamage += EnemyController_OnTakeDamage;
    }

    private void EnemyController_OnTakeDamage(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_TAKE_DAMAGE);
    }

    private void EnemyController_OnDeath(object sender, EventArgs e)
    {
        enemyController.isDead = true;
        animator.SetBool(IS_DEATH, true);
    }

    private void EnemyController_OnAttacking(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_ATTACKING);
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, enemyController.isWalking);

    }

    public void OnAttackAnimationFinished()
    {
        print("Filler");
    }

    public void OnDeathAnimationFinished()
    {
        
        enemyController.Die();
    }
}
