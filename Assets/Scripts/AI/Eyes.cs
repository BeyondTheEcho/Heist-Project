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
        [SerializeField] private bool m_DrawDebugRays = false;

        private GameObject m_Player;

        private const int c_AllLayers = ~0;

        private void Awake()
        {
            //Caches
            m_Player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            //Draws debug rays represent the npc's field of view if the bool is checked in the inspector
            if (m_DrawDebugRays) DrawDebugRays();
        }


        /// <summary>
        /// Draws Debug Rays. Blue for transform.forward and red to indicate the arc of the set ViewArc
        /// </summary>
        private void DrawDebugRays()
        {
            Vector3 leftOffset = Quaternion.Euler(0f, -m_ViewArcInDegrees, 0f) * transform.forward;
            Vector3 rightOffset = Quaternion.Euler(0f, m_ViewArcInDegrees, 0f) * transform.forward;

            Debug.DrawRay(transform.position, transform.forward * m_ViewDistance, Color.blue);
            Debug.DrawRay(transform.position, leftOffset * m_ViewDistance, Color.red);
            Debug.DrawRay(transform.position, rightOffset * m_ViewDistance, Color.red);
        }

        /// <summary>
        /// Checks if the angle from the transform.forward vector is within the range set by the serialized ViewArc float
        /// </summary>
        /// <param name="directionToPlayer">Directional Vector3 that should be in local coordinates AND normalized</param>
        /// <returns>Returns true if within the arc, false otherwise</returns>
        private bool CheckIfInViewArc(Vector3 directionToPlayer)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            //Debug.Log(angleToPlayer);

            return angleToPlayer < m_ViewArcInDegrees;
        }

        public void ReportSensorData(SensorData data)
        {
            //Calculates a normalized directional vector to the player from the npc's eyes
            Vector3 directionToPlayer = (m_Player.transform.position - m_EyesTransform.position).normalized;

            //Avoids a raycast if not required because the player is not in the npc's field of view
            if (CheckIfInViewArc(directionToPlayer))
            {
                //Raycasts from the eyes of the npc to the player and if successful populates hit and enters the if statement (occurs on all layers)
                if (Physics.Raycast(m_EyesTransform.position, directionToPlayer, out RaycastHit hit, m_ViewDistance, c_AllLayers))
                {
                    //If the hit object was in fact the player, meaning the view is unobstructed (clear line of sight)
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log($"{gameObject.name}: Has LOS");

                        //Populated the passed in ref type
                        data.m_HasLineOfSightToPlayer = true;
                        return;
                    }
                }
            }

            Debug.Log($"{gameObject.name}: Does NOT have LOS");
            //populates the passed in ref type if the above fails
            data.m_HasLineOfSightToPlayer = false;
        }
    }
}