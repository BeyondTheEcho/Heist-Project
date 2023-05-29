using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] private float m_MoveSpeed = 3.5f;

        private CharacterController m_Controller;
        private Animator m_Animator;

        private void Awake()
        {
            m_Controller = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            Debug.Log(direction);

            ResetAllBools();
            UpdateAnimatorBools(direction);

            m_Controller.Move(direction * m_MoveSpeed * Time.deltaTime);
        }

        private void UpdateAnimatorBools(Vector3 direction)
        {
            if (direction.magnitude == 0f) m_Animator.SetBool("IsIdle", true);
            if (direction.magnitude > 0f) m_Animator.SetBool("IsWalking", true);
            if (direction.z < 0f) m_Animator.SetBool("IsBackwards", true);
            if (direction.x > 0f) m_Animator.SetBool("IsRightStrafing", true);
            if (direction.x < 0f) m_Animator.SetBool("IsLeftStrafing", true);
        }

        private void ResetAllBools()
        {
            m_Animator.SetBool("IsIdle", false);
            m_Animator.SetBool("IsArmed", true);
            m_Animator.SetBool("IsWalking", false);
            m_Animator.SetBool("IsRunning", false);
            m_Animator.SetBool("IsBackwards", false);
            m_Animator.SetBool("IsRightStrafing", false);
            m_Animator.SetBool("IsLeftStrafing", false);
        }
    }
}
