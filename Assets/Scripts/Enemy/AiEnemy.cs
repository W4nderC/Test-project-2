using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiEnemy : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    public Transform target;
    public float attackDistance;

    private NavMeshAgent m_agent;
    [SerializeField] private Animator animator;
    private float m_Distance;

    private void OnEnable()
    {
        enemyController.isDead = false;
    }

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        m_agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        if (enemyController.isDead) return;
        
        m_Distance = Vector3.Distance(m_agent.transform.position, target.position);
        if (m_Distance < attackDistance)
        {
            m_agent.isStopped = true;
            enemyController.isWalking = false;
        }
        else
        {
            m_agent.isStopped = false;
            enemyController.isWalking = true;

            m_agent.destination = target.position;
        }
    }
}
