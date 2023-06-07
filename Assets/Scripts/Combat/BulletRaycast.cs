using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{
    [SerializeField] private GameObject m_HitEffectPrefab;
    [SerializeField] private float m_Speed = 200f;

    private Vector3 m_TargetPosition;

    // Update is called once per frame
    void Update()
    {
        float distanceBeforeMove = Vector3.Distance(transform.position, m_TargetPosition);

        Vector3 moveDirection = (m_TargetPosition - transform.position).normalized;

        transform.position += moveDirection * m_Speed * Time.deltaTime;

        float distanceAfterMove = Vector3.Distance(transform.position, m_TargetPosition);

        if (distanceBeforeMove < distanceAfterMove)
        {
            Instantiate(m_HitEffectPrefab, transform.position, Quaternion.LookRotation(moveDirection, Vector3.up));
            Destroy(gameObject);
        }
    }

    public void SetupBullet(Vector3 targetPosition)
    {
        m_TargetPosition = targetPosition;
    }
}
