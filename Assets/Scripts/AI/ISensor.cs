using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface ISensor
    {
        public abstract void ReportSensorData(SensorData data);
    }
}