using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    public float attackCooldown = 2f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    [SerializeField] private Collider col;

    void OnEnable()
    {
        col.enabled = true;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        IAllyHumanoid[] humanoids = other.GetComponents<IAllyHumanoid>();
        if (humanoids != null && humanoids.Length > 0)
        {
            foreach (IAllyHumanoid humanoid in humanoids)
            {
                // Check if the humanoid is within attack range
                if (enemyController.isTakeDamage || enemyController.isWalking || enemyController.isDead) return;
                enemyController.MeleeAttack();
                humanoid.TakeDamage(enemyController.damage);

            }
            lastAttackTime = Time.time;
        }
    }
}
