using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    public float attackCooldown = 1f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {

            PlayerController enemy = other.GetComponent<PlayerController>();
            if (enemy != null)
            {
                enemy.TakeDamage(enemyController.damage);
                enemyController.MeleeAttack();
            }
            lastAttackTime = Time.time;
        }
    }
}
