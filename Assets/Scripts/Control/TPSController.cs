using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class TPSController : MonoBehaviour
{
    [SerializeField] private GameObject m_MuzzleFlashPrefab;
    [SerializeField] private AudioClip m_RifleAudioClip;
    [SerializeField] private AudioSource m_RifleAudioSource;
    [SerializeField] private Rig m_AimRig;
    [SerializeField] private Rig m_IdleRig;
    [SerializeField] private CinemachineVirtualCamera m_AimCamera;
    [SerializeField] private float m_Sensitivity = 1f;
    [SerializeField] private float m_AimSensitivity = 0.5f;
    [SerializeField] private LayerMask m_AimLayerMask;
    [SerializeField] private Transform m_AimTarget;
    [SerializeField] private BulletRaycast m_Bullet;
    [SerializeField] private Transform m_BulletSpawnPosition;
    [SerializeField] private float m_TimeBetweenShots = 0.01f;

    private float m_TimeSinceLastShot = Mathf.Infinity;
    private Animator m_Animator;
    private ThirdPersonController m_ThirdPersonController;
    private StarterAssetsInputs m_StarterAssetsInputs;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_ThirdPersonController = GetComponent<ThirdPersonController>();
        m_StarterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        m_MuzzleFlashPrefab.SetActive(false);
        Debug.Log(m_StarterAssetsInputs.m_Shoot);

        m_TimeSinceLastShot += Time.deltaTime;

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
            m_Animator.SetBool("IsAiming", true);
            m_AimRig.weight = 1f;
            m_IdleRig.weight = 0f;
            m_AimCamera.gameObject.SetActive(true);
            m_ThirdPersonController.SetSensitivity(m_AimSensitivity);
            m_ThirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = aimPosition;
            worldAimTarget.y = transform.position.y;

            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (m_StarterAssetsInputs.m_Shoot)
            {
                if (m_TimeSinceLastShot < m_TimeBetweenShots) return;

                Vector3 BulletAimDirection = (aimPosition - m_BulletSpawnPosition.transform.position).normalized;

                m_MuzzleFlashPrefab.SetActive(true);
                m_RifleAudioSource.PlayOneShot(m_RifleAudioClip);
                BulletRaycast bullet = Instantiate(m_Bullet, m_BulletSpawnPosition.transform.position, Quaternion.LookRotation(BulletAimDirection));
                bullet.SetupBullet(hit.point);

                m_TimeSinceLastShot = 0f;
            }
        }
        else
        {
            m_Animator.SetBool("IsAiming", false);
            m_AimRig.weight = 0f;
            m_IdleRig.weight = 1f;
            m_AimCamera.gameObject.SetActive(false);
            m_ThirdPersonController.SetSensitivity(m_Sensitivity);
            m_ThirdPersonController.SetRotateOnMove(true);
        }
    }
}
