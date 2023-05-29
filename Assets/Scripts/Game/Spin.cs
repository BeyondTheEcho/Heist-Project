using UnityEngine;

namespace Game
{
    public class Spin : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = Quaternion.Euler(0, Time.time * 90, 0);
        }
    }
}
