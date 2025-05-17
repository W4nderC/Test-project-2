using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public float attackCooldown = 1f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && Time.time >= lastAttackTime + attackCooldown)
        {

            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerController.damage);
                playerController.MeleeAttack();
            }
            lastAttackTime = Time.time;
        }
    }
}
