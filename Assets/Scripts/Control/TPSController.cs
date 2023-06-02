using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class TPSController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_AimCamera;

    private StarterAssetsInputs m_StarterAssetsInputs;

    private void Awake()
    {
        m_StarterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (m_StarterAssetsInputs.m_Aim)
        {
            m_AimCamera.gameObject.SetActive(true);
        }
        else
        {
            m_AimCamera.gameObject.SetActive(false);
        }
    }
}
