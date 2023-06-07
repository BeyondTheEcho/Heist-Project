using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_Damage = 5f;
    [SerializeField] private float m_Speed = 10f;
    [SerializeField] private float m_BulletLifetime = 15f;

    private Rigidbody m_Rigidbody;
    private float m_TimeSinceSpawned = 0f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }

    private void Update()
    {
        m_TimeSinceSpawned += Time.deltaTime;

        if (m_TimeSinceSpawned >= m_BulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"{collider.gameObject.name}");
        Destroy(gameObject);
    }
}
