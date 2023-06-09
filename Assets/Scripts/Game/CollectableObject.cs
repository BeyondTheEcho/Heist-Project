using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class CollectableObject : MonoBehaviour
    {
        public string DisplayName;
        public event Action Collected = () => { };
        
        private bool _isOverlap;
        private float _timeHeld;
        
        void Update()
        {
            if (!_isOverlap || !Input.GetKey(KeyCode.F))
            {
                _timeHeld = 0f;
                return;
            }
            
            _timeHeld += Time.deltaTime;

            if (_timeHeld >= 1f)
            {
                Collected();
                CollectableHint.Hide();
                Destroy(gameObject);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _isOverlap = true;
            CollectableHint.Show(DisplayName);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _isOverlap = false;
            CollectableHint.Hide();
        }

        void OnValidate()
        {
            Assert.IsNotNull(GetComponent<Collider>(), $"{nameof(CollectableObject)} requires a collider.");
            Assert.IsTrue(GetComponent<Collider>().isTrigger, $"{nameof(CollectableObject)}'s collider must be set to trigger.");
        }
    }
}
