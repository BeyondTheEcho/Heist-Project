using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitscan : MonoBehaviour
{
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
        //}
    }
    
    public void Shoot()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 10f, false);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.transform.gameObject.name);
            hit.transform.GetComponent<Health>()?.TakeDamage(100);
        }
    }
}
