using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public float attackCooldown = 2f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;
    [SerializeField] private Collider col;

    private void OnEnable()
    {
        col = GetComponent<Collider>();
        EnableCollider();
    }

    private void OnTriggerStay(Collider other)
    {
        // Check cooldown
        if (Time.time < lastAttackTime + attackCooldown) return;
        

        IEnemyHumanoid[] humanoids = other.GetComponents<IEnemyHumanoid>();
        if (humanoids != null && humanoids.Length > 0)
        {
            if (playerController.isInvincible) return;
            playerController.isInvincible = true;

            foreach (IEnemyHumanoid enemy in humanoids)
            {
                playerController.MeleeAttack();
                enemy.TakeDamage(playerController.damage);
            }
            lastAttackTime = Time.time;
        }
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }
}
