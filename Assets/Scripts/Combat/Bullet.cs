using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_Speed = 100f;

    public void SetTarget(Transform target)
    {
        transform.LookAt(target.position);

        GetComponent<Rigidbody>().MovePosition((target.position - transform.position) * m_Speed);
    }
}
