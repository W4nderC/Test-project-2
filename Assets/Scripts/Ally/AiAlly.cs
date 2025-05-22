using System;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAlly  : MonoBehaviour
{
    [SerializeField] private AllyController allyController;

    public Transform target;
    public float attackDistance;

    private NavMeshAgent m_agent;
    private float m_Distance;


    public float detectionRadius = 10f;
    // public string enemyTag = "Enemy";
    private Transform currentTarget;
    public enum EnemyTags
    {
        Enemy,
    }
    public EnemyTags enemyTags;

    void Start()
    {
        allyController = GetComponent<AllyController>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (allyController.isDead) return;


        FindNearestEnemy();

        if (currentTarget == null) return;
        
        m_Distance = Vector3.Distance(m_agent.transform.position, currentTarget.position);

        // Check if player is within attack distance
        if (m_Distance < attackDistance)
        {
            m_agent.isStopped = true;
            allyController.isWalking = false;

            if (currentTarget != null)
            {
                // face the enemy
                Vector3 direction = (currentTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            m_agent.isStopped = false;
            m_agent.destination = currentTarget.position;
            
            allyController.isWalking = true;
        }
    }
    

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTags.ToString());
        // if no enemies found, switch to the next tag
        if (enemies.Length == 0)
        {
            currentTarget = null;

            if ((int)enemyTags == Enum.GetValues(typeof(EnemyTags)).Length - 1) return;

            enemyTags++;
            return;
        }

        GameObject nearest = enemies
            .Where(e => Vector3.Distance(transform.position, e.transform.position) <= detectionRadius)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .FirstOrDefault();

        currentTarget = nearest != null ? nearest.transform : null;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
}
