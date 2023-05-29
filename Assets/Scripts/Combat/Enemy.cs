using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject m_FiringPos;
    [SerializeField] private Bullet m_BulletPrefab;
    [SerializeField] private float m_AttackRange = 50f;
    [SerializeField] private float m_FiringDelay = 3f;

    private GameObject m_Player;
    private float m_TimeSinceFired = 0f;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_TimeSinceFired += Time.deltaTime;

        if (Vector3.Distance(transform.position, m_Player.transform.position) > m_AttackRange) return;

        transform.LookAt(m_Player.transform);

        if (m_TimeSinceFired < m_FiringDelay) return;

        Bullet bullet = Instantiate(m_BulletPrefab, m_FiringPos.transform.position, Quaternion.identity);

        bullet.SetTarget(m_Player.transform);

        m_TimeSinceFired = 0f;
    }
}
