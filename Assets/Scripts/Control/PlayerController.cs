using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] private float m_MoveSpeed = 3.5f;
        [SerializeField] [Range(0, 10)] private float m_RunMoveSpeed = 3.5f;
        [SerializeField] [Range(0, 5)] private float m_TurnSpeed = 1.5f;
        [SerializeField] private Transform m_PlayerChestTransform;

        private Animator m_Animator;

        private bool m_IsGrounded = true;

        private bool IsRunning => Input.GetKey(KeyCode.LeftShift);
        
        #region Animator Hashes
        private static readonly int Anim_IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int Anim_IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Anim_IsBackwards = Animator.StringToHash("IsBackwards");
        private static readonly int Anim_IsRightStrafing = Animator.StringToHash("IsRightStrafing");
        private static readonly int Anim_IsLeftStrafing = Animator.StringToHash("IsLeftStrafing");
        private static readonly int Anim_IsArmed = Animator.StringToHash("IsArmed");
        private static readonly int Anim_IsRunning = Animator.StringToHash("IsRunning");
        #endregion

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_Animator.SetBool(Anim_IsArmed, true);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                m_IsGrounded = true;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
            {
                GetComponent<Rigidbody>().velocity += Vector3.up * 5f;
                m_IsGrounded = false;
            }
            
            var v = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(Vector3.up, v.x * m_TurnSpeed);
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 absoluteDirection = new Vector3(horizontal, 0f, vertical);
            absoluteDirection.Normalize();

            UpdateAnimatorBools(absoluteDirection);

            transform.Translate(absoluteDirection * (Time.deltaTime * (IsRunning ? m_RunMoveSpeed : m_MoveSpeed)));
        }

        private void UpdateAnimatorBools(Vector3 direction)
        {
            m_Animator.SetBool(Anim_IsIdle, direction.magnitude == 0f);
            m_Animator.SetBool(Anim_IsWalking, direction.magnitude > 0f);
            m_Animator.SetBool(Anim_IsRunning, IsRunning);
            m_Animator.SetBool(Anim_IsBackwards, direction.z < 0f);
            m_Animator.SetBool(Anim_IsRightStrafing, direction.x > 0f);
            m_Animator.SetBool(Anim_IsLeftStrafing, direction.x < 0f);
        }

        public Transform GetTargetTransform()
        {
            return m_PlayerChestTransform;
        }
    }
}
