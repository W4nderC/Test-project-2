using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public float attackCooldown = 1f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    private void OnTriggerStay(Collider other)
    {
        IEnemyHumanoid humanoid = other.GetComponent<IEnemyHumanoid>();
        if (humanoid != null && Time.time >= lastAttackTime + attackCooldown)
        {

            // EnemyController enemy = other.GetComponent<EnemyController>();
            // if (enemy != null)
            // {
            //     playerController.MeleeAnimation();
            //     enemy.TakeDamage(playerController.damage);

            // }
            humanoid.MeleeAnimation();
            humanoid.TakeDamage(playerController.damage);
            
            lastAttackTime = Time.time;
        }
    }
}
