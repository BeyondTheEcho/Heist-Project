using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace AI
{
    [RequireComponent(typeof(Eyes))]
    public class PNI_Brain : MonoBehaviour
    {
        [SerializeField] private PatrolRoute m_PatrolRoute;
        [SerializeField] private float m_SensorUpdateRateInSeconds = 1f;
        [SerializeField] private float m_CoverSearchRadius = 10f;

        private ISensor[] m_Sensors;
        private WaitForSeconds m_WaitForSeconds;
        private SensorData m_SensorData = new SensorData();
        private GuardState m_GuardState = GuardState.Patrol;
        private NavMeshAgent m_NavMeshAgent;
        private GameObject m_Player;
        private Movement m_Movement;
        private Animator m_Animator;

        private int m_CurrentPatrolPointIndex = 0;
        private float m_PatrolPointTolerance = 1f;
        private float m_TimeLingering = 0f;
        private float m_LingerTime = -1f;
        private bool m_InCover = false;

        //Consts
        private const int c_AllLayers = ~0;

        void Awake()
        {
            //Caches
            m_Animator = GetComponent<Animator>();
            m_Movement = GetComponent<Movement>();
            m_Player = GameObject.FindGameObjectWithTag("Player");
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_Sensors = GetComponents<ISensor>();
            m_WaitForSeconds = new WaitForSeconds(m_SensorUpdateRateInSeconds);
        }

        void Start()
        {
            StartCoroutine(GetSensorData());
        }


        void Update()
        {
            UpdateState();

            switch (m_GuardState)
            {
                case GuardState.Patrol:
                    PatrolBehaviour();
                    break;
                case GuardState.Guard:
                    //do guard stuff
                    break;
                case GuardState.Attack:
                    AttackBehaviour();
                    break;
            }
        }

        private void AttackBehaviour()
        {
            //Fire two shots at player

            if (!m_InCover) MoveToSafeCover();
        }

        private void PatrolBehaviour()
        {
            if (m_LingerTime == -1f)
            {
                m_LingerTime = m_PatrolRoute.GetLingerTime();
                m_Movement.MoveToPosition(GetPatrolPoint(), m_PatrolPointTolerance);
            }

            if (ReachedWaypoint())
            {
                //m_Animator.SetBool("IsMoving", false);

                if (m_TimeLingering >= m_LingerTime)
                {
                    m_TimeLingering = 0f;

                    m_CurrentPatrolPointIndex = m_PatrolRoute.GetNextIndex(m_CurrentPatrolPointIndex);

                    m_Movement.MoveToPosition(GetPatrolPoint(), m_PatrolPointTolerance);
                }
                else
                {
                    m_TimeLingering += Time.deltaTime;
                }
            }
        }

        private void MoveToSafeCover()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_CoverSearchRadius);

            Collider[] sortedColliders = colliders.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

            foreach (Collider collider in sortedColliders)
            {
                if (collider.gameObject.tag == "Cover")
                {
                    if (IsCoverSafe(collider.gameObject))
                    {
                        m_NavMeshAgent.SetDestination(collider.gameObject.transform.position);
                        m_InCover = true;
                        break;
                    }
                }
            }
        }

        private bool IsCoverSafe(GameObject cover)
        {
            Vector3 directionToPlayer = (m_Player.transform.position - cover.transform.position).normalized;

            if (Physics.Raycast(cover.transform.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity, c_AllLayers))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ReachedWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetPatrolPoint());
            return distanceToWaypoint < m_PatrolPointTolerance;
        }

        private Vector3 GetPatrolPoint()
        {
            return m_PatrolRoute.GetPatrolPoint(m_CurrentPatrolPointIndex);
        }

        private void UpdateState()
        {
            if (m_GuardState == GuardState.Patrol || m_GuardState == GuardState.Guard)
            {
                if (IsPlayerDetected())
                {
                    m_GuardState = GuardState.Attack;
                }
            }

            if (m_GuardState == GuardState.Patrol)
            {
                if (m_PatrolRoute == null)
                {
                    m_GuardState = GuardState.Guard;
                }
            }

            if (m_GuardState == GuardState.Guard)
            {
                //Do guard stuff
            }

            if (m_GuardState == GuardState.Attack)
            {
                //Do other stuff
            }
        }

        private bool IsPlayerDetected()
        {
            if (m_SensorData.m_IsPlayerInPhysicalDetectionRange) return true;
            if (m_SensorData.m_HasLineOfSightToPlayer) return true;

            return false;
        }


        private IEnumerator GetSensorData()
        {
            while (true)
            {
                foreach (ISensor sensor in m_Sensors)
                {
                    sensor.ReportSensorData(m_SensorData);
                }

                yield return m_WaitForSeconds;
            }
        }
    }

    public enum GuardState
    {
        Patrol,
        Guard,
        Attack,
    }
}
