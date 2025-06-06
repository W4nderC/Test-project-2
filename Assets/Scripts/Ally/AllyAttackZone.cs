using UnityEngine;

public class AllyAttackZone : MonoBehaviour
{
    [SerializeField] private AllyController allyController;

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

        IEnemyHumanoid[] humanoids = other.GetComponents<IEnemyHumanoid>();
        if (humanoids != null && humanoids.Length > 0)
        {
            foreach (IEnemyHumanoid humanoid in humanoids)
            {
                // Check if the humanoid is within attack range
                if (allyController.isTakeDamage || allyController.isWalking || allyController.isDead) return;

                allyController.MeleeAttack();
                humanoid.TakeDamage(allyController.damage);
            }
            lastAttackTime = Time.time;
        }
    }
}
