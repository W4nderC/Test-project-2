using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public float attackRange = 1.5f;
    public LayerMask enemyLayer;
    public int damage = 10;

    void Update()
    {
        HandleMovement();
        if (Input.GetMouseButtonDown(0)) // một tay - chạm để đánh
        {
            MeleeAttack();
        }
    }

    void HandleMovement()
    {
        Vector2 input = new Vector2(Input.acceleration.x, Input.acceleration.y);
        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.Translate(move * moveSpeed * Time.deltaTime);
        if (move != Vector3.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    void MeleeAttack()
    {
        animator.SetTrigger("attack");
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
