using System;
using UnityEngine;

namespace Game.Objectives
{
    public abstract class ObjectiveBase : MonoBehaviour
    {
        public event Action StateUpdated = () => { };
        public bool IsComplete { get; protected set; }

        public abstract string GenerateText();
        
        protected void RaiseStateUpdated()
        {
            StateUpdated();
        }
    }
}
