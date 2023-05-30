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

        void Awake()
        {
            m_Sensors = GetComponents<ISensor>();
            m_WaitForSeconds = new WaitForSeconds(m_SensorUpdateRateInSeconds);
        }

        void Start()
        {
            StartCoroutine(GetSensorData());
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
}
