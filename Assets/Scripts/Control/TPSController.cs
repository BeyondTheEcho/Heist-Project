using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class TPSController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_AimCamera;
    [SerializeField] private float m_Sensitivity = 1f;
    [SerializeField] private float m_AimSensitivity = 0.5f;
    [SerializeField] private LayerMask m_AimLayerMask;
    [SerializeField] private Transform m_AimTarget;

    private ThirdPersonController m_ThirdPersonController;
    private StarterAssetsInputs m_StarterAssetsInputs;

    private void Awake()
    {
        m_ThirdPersonController = GetComponent<ThirdPersonController>();
        m_StarterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        Vector3 aimPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_AimLayerMask))
        {
            m_AimTarget.position = hit.point;
            aimPosition = hit.point;
        }


        if (m_StarterAssetsInputs.m_Aim)
        {
            m_AimCamera.gameObject.SetActive(true);
            m_ThirdPersonController.SetSensitivity(m_AimSensitivity);
            m_ThirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = aimPosition;
            worldAimTarget.y = transform.position.y;

            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            m_AimCamera.gameObject.SetActive(false);
            m_ThirdPersonController.SetSensitivity(m_Sensitivity);
            m_ThirdPersonController.SetRotateOnMove(true);
        }
    }
}
