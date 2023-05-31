using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(Eyes))]
    public class PNI_Brain : MonoBehaviour
    {
        [SerializeField] private float m_SensorUpdateRateInSeconds = 1f;

        private ISensor[] m_Sensors;
        private WaitForSeconds m_WaitForSeconds;
        private SensorData m_SensorData = new SensorData();
        private GuardState m_GuardState = GuardState.Patrol;

        void Awake()
        {
            m_Sensors = GetComponents<ISensor>();
            m_WaitForSeconds = new WaitForSeconds(m_SensorUpdateRateInSeconds);
        }

        void Start()
        {
            StartCoroutine(GetSensorData());
        }


        void Update()
        {
            if (m_GuardState == GuardState.Patrol)
            {
                //do patrol stuff
            }
            else if (m_GuardState == GuardState.Guard)
            {
                //do guard stuff
            }
            else if (m_GuardState == GuardState.Attack)
            {
                //do attack stuff
            }
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
