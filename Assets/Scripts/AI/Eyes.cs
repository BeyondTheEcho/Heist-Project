using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace AI
{
    public class Eyes : MonoBehaviour, ISensor
    {
        [SerializeField] private float m_ViewArcInDegrees = 45f;
        [SerializeField] private float m_ViewDistance = 25f;
        [SerializeField] private Transform m_EyesTransform;

        private GameObject m_Player;

        private const int c_AllLayers = ~0;

        private void Awake()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
        }

        private bool CheckIfInViewArc(Vector3 directionToPlayer)
        {
            float angleToPlayer = Vector3.Angle(Vector3.forward, -directionToPlayer);

            //Debug.Log(angleToPlayer);

            return angleToPlayer < m_ViewArcInDegrees;
        }

        public void ReportSensorData(SensorData data)
        {
            Vector3 dirToPlayer = m_Player.transform.position - m_EyesTransform.position;

            if (CheckIfInViewArc(dirToPlayer))
            {
                if (Physics.Raycast(m_EyesTransform.position, dirToPlayer, out RaycastHit hit, m_ViewDistance, c_AllLayers))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log($"{gameObject.name}: Has LOS");

                        data.m_HasLineOfSightToPlayer = true;
                        return;
                    }
                }
            }

            Debug.Log($"{gameObject.name}: Does NOT have LOS");
            data.m_HasLineOfSightToPlayer = false;
        }
    }
}