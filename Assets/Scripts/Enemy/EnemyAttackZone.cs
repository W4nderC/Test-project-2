using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    public float attackCooldown = 2f; // cooldown time between attacks
    private float lastAttackTime = -Mathf.Infinity;

    private void OnTriggerStay(Collider other)
    {
        IAllyHumanoid humanoid = other.GetComponent<IAllyHumanoid>();
        if (humanoid != null && Time.time >= lastAttackTime + attackCooldown)
        {

            // PlayerController player = other.GetComponent<PlayerController>();
            // if (player != null)
            // {
            //     enemyController.MeleeAnimation();
            //     player.TakeDamage(enemyController.damage);
            // }
            humanoid.MeleeAnimation();
            humanoid.TakeDamage(enemyController.damage);
            
            lastAttackTime = Time.time;
        }
    }
}
