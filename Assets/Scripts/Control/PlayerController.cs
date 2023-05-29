using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] private float m_MoveSpeed = 3.5f;
        [SerializeField] [Range(0, 5)] private float m_TurnSpeed = 1.5f;
        [SerializeField] private Transform m_PlayerChestTransform;

        private CharacterController m_Controller;
        private Animator m_Animator;
        
        #region Animator Hashes
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsBackwards = Animator.StringToHash("IsBackwards");
        private static readonly int IsRightStrafing = Animator.StringToHash("IsRightStrafing");
        private static readonly int IsLeftStrafing = Animator.StringToHash("IsLeftStrafing");
        private static readonly int IsArmed = Animator.StringToHash("IsArmed");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        #endregion

        private void Awake()
        {
            m_Controller = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            var v = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(Vector3.up, v.x * m_TurnSpeed);
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 absoluteDirection = new Vector3(horizontal, 0f, vertical);
            absoluteDirection.Normalize();
            Vector3 relativeDirection = horizontal * transform.right + vertical * transform.forward;
            relativeDirection.Normalize();

            ResetAllBools();
            UpdateAnimatorBools(absoluteDirection);

            m_Controller.Move(relativeDirection * (m_MoveSpeed * Time.deltaTime));
        }

        private void UpdateAnimatorBools(Vector3 direction)
        {
            if (direction.magnitude == 0f) m_Animator.SetBool(IsIdle, true);
            if (direction.magnitude > 0f) m_Animator.SetBool(IsWalking, true);
            if (direction.z < 0f) m_Animator.SetBool(IsBackwards, true);
            if (direction.x > 0f) m_Animator.SetBool(IsRightStrafing, true);
            if (direction.x < 0f) m_Animator.SetBool(IsLeftStrafing, true);
        }

        private void ResetAllBools()
        {
            m_Animator.SetBool(IsIdle, false);
            m_Animator.SetBool(IsArmed, true);
            m_Animator.SetBool(IsWalking, false);
            m_Animator.SetBool(IsRunning, false);
            m_Animator.SetBool(IsBackwards, false);
            m_Animator.SetBool(IsRightStrafing, false);
            m_Animator.SetBool(IsLeftStrafing, false);
        }

        public Transform GetTargetTransform()
        {
            return m_PlayerChestTransform;
        }
    }
}
