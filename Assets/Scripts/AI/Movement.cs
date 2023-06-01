using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Movement : MonoBehaviour
    {
        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;

        private Vector3 m_LastPosition;
        private float m_MovementTolerance = 0.01f;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_LastPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimatorIfMoving();
        }

        private void UpdateAnimatorIfMoving()
        {
            if (Vector3.Distance(transform.position, m_LastPosition) > m_MovementTolerance)
            {
                m_Animator.SetBool("IsMoving", true);
                m_LastPosition = transform.position;
            }
            else
            {
                m_Animator.SetBool("IsMoving", false);
                m_LastPosition = transform.position;
            }
        }

        public void MoveToPosition(Vector3 position, float searchRadius)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, searchRadius, NavMesh.AllAreas))
            {
                m_NavMeshAgent.SetDestination(navMeshHit.position);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Failed to find a valid Nav Mesh Position");
            }
        }
    }
}
