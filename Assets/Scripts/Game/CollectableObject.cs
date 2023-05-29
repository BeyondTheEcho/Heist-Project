using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class CollectableObject : MonoBehaviour
    {
        public event Action Collected = () => { };
        
        private bool _isOverlap;
        private float _timeHeld;
        
        void Update()
        {
            if (!_isOverlap || !Input.GetKey(KeyCode.Space))
            {
                _timeHeld = 0f;
                return;
            }
            
            _timeHeld += Time.deltaTime;

            if (_timeHeld >= 1f)
            {
                Collected();
                Destroy(gameObject);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _isOverlap = true;
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _isOverlap = false;
            _timeHeld = 0f;
        }

        void OnValidate()
        {
            Assert.IsNotNull(GetComponent<Collider>(), $"{nameof(CollectableObject)} requires a collider.");
            Assert.IsTrue(GetComponent<Collider>().isTrigger, $"{nameof(CollectableObject)}'s collider must be set to trigger.");
        }
    }
}
