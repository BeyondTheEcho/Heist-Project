using System.Collections;
using System.Collections.Generic;
using Control;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject m_FiringPos;
    [SerializeField] private Bullet m_BulletPrefab;
    [SerializeField] private float m_AttackRange = 50f;
    [SerializeField] private float m_FiringDelay = 0.01f;

    private GameObject m_Player;
    private float m_TimeSinceFired = 0f;
    private bool m_HasLineOfSight = false;

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
        if (!m_HasLineOfSight) return;
        
        Bullet bullet = Instantiate(m_BulletPrefab, m_FiringPos.transform.position, transform.rotation);

        bullet.SetTarget(m_Player.GetComponent<PlayerController>().GetTargetTransform());

        m_TimeSinceFired = 0f;
    }

    void LateUpdate()
    {
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(m_FiringPos.transform.position, m_Player.transform.position - transform.position, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Has LOS");
                m_HasLineOfSight = true;
            }
        }
        else
        {
            m_HasLineOfSight = false;
        }
    }
}
