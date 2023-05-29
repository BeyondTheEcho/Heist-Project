using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private float m_xMin = -25f;
    [SerializeField] private float m_xMax = 25f;


    // Update is called once per frame
    void Update()
    {
        float xVal = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(Vector3.left, xVal);
    }
}
