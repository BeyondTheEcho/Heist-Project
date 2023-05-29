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
        var rot = transform.rotation.eulerAngles;

        float xVal = rot.x + Input.GetAxisRaw("Mouse Y");

        transform.rotation = Quaternion.Euler(Mathf.Clamp(xVal, m_xMin, m_xMax), 0 ,0);
    }
}
