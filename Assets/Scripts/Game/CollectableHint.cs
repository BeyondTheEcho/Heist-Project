using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class CollectableHint : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        
        public static void Show(string name)
        {
            var obj = FindObjectOfType<CollectableHint>();
            Assert.IsNotNull(obj, $"Failed to find a {nameof(CollectableHint)}!");

            obj.Text.enabled = true;
            obj.Text.text = $"Hold space to collect \"{name}\"...";
        }

        public static void Hide()
        {
            var obj = FindObjectOfType<CollectableHint>();
            Assert.IsNotNull(obj, $"Failed to find a {nameof(CollectableHint)}!");

            obj.Text.enabled = false;
        }
    }
}
