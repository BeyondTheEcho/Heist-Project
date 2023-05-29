using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float m_Health = 100f;

    private void Update()
    {
        DieIfNoHealth();
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;

        Debug.Log($"{gameObject.name}: took {damage} damage.");
    }

    public void DieIfNoHealth()
    {
        if (m_Health <= 0)
        {
            Debug.Log("The Player has died");
            Destroy(gameObject);
        }
    }
}
