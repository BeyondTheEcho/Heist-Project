using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AI
{
    public class Somatic : MonoBehaviour, ISensor
    {
        [Header("Somatic Settings")]
        [SerializeField] private float m_PhysicalDetectionRadius = 2.5f;

        [Header("Debug Settings")]
        [SerializeField] private bool m_EnableLogging = false;
        [SerializeField] private bool m_DrawDetectionSphere = false;
        [SerializeField] private ColorStruct m_DebugSphereColor = new(33f, 107f, 255f, 0.25f);

        //Private vars
        private GameObject m_Player;

        private void Awake()
        {
            //Caches
            m_Player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Draws a gizmo sphere representing the physical detection radius if enabled in inspector
        /// </summary>
        void OnDrawGizmos()
        {
            if (!m_DrawDetectionSphere) return;

            //Sets sphere color to converted helper struct to convert standard RGBA to unity float values
            Gizmos.color = m_DebugSphereColor.ToColor();
            Gizmos.DrawSphere(transform.position, m_PhysicalDetectionRadius);
        }

        /// <summary>
        /// Reports somatic data by populating the passed in ref type
        /// </summary>
        /// <param name="data">populated based on the input from the sensors</param>
        public void ReportSensorData(SensorData data)
        {
            //Full exact distance calculation to determine the exact distance to the player
            if (Vector3.Distance(transform.position, m_Player.transform.position) <= m_PhysicalDetectionRadius)
            {
                if (m_EnableLogging)
                {
                    Debug.Log($"{gameObject.name}: Player in Physical Detection Range");
                }

                //populated passed in ref type and returns early
                data.m_IsPlayerInPhysicalDetectionRange = true;
                return;
            }

            if (m_EnableLogging)
            {
                Debug.Log($"{gameObject.name}: Player NOT in Physical Detection Range");
            }

            //populated passed in ref type
            data.m_IsPlayerInPhysicalDetectionRange = false;
        }
    }
}