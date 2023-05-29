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

    private Vector3? m_TargetPos = null;

    private float m_TimeSinceSpawned = 0f;

    private void Update()
    {
        m_TimeSinceSpawned += Time.deltaTime;

        if (m_TimeSinceSpawned >= m_BulletLifetime)
        {
            Destroy(gameObject);
        }

        if (m_TargetPos != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos.Value, m_Speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform player)
    {
        transform.LookAt(player);
        
        m_TargetPos = player.position;
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
