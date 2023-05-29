using System;
using UnityEngine;

namespace Game.Objectives
{
    public abstract class ObjectiveBase : MonoBehaviour
    {
        public event Action StateUpdated = () => { };
        public abstract bool IsComplete { get; }

        public abstract string GenerateText();
        
        protected void RaiseStateUpdated()
        {
            StateUpdated();
        }
    }
}
