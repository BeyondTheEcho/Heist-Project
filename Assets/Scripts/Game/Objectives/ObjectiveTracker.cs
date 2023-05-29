using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Objectives
{
    public class ObjectiveTracker : MonoBehaviour
    {
        private IEnumerable<ObjectiveBase> AllObjectives => Enumerable.Repeat(Major, 1).Concat(Minor);

        public TextMeshProUGUI UI;
        public ObjectiveBase Major;
        public ObjectiveBase[] Minor;

        void Start()
        {
            _updateText();
            foreach (var obj in AllObjectives)
            {
                obj.StateUpdated += _updateText;
            }
        }

        private void _updateText()
        {
            UI.text = _generateMessage();
        }
        
        private string _generateMessage()
        {
            return string.Join("\n", AllObjectives.Select(obj => obj.GenerateText()));
        }
        
        private void OnValidate()
        {
            Assert.IsFalse(Minor.Contains(Major), "Major objective must not be in minor objective list.");
            Assert.IsTrue(Minor.Length == Minor.Distinct().Count(), "Duplicate minor objective found.");
        }
    }
}
