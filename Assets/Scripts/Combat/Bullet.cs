using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_Damage = 5f;
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_BulletLifetime = 15f;

    private float m_TimeSinceSpawned = 0f;

    private void Update()
    {
        m_TimeSinceSpawned += Time.deltaTime;

        if (m_TimeSinceSpawned >= m_BulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform player)
    {
        transform.LookAt(player.transform);

        GetComponent<Rigidbody>().AddForce(Vector3.forward * m_Speed);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Health>().TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
