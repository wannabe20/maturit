using UnityEngine;
using UnityEngine.UI; // Import UI library

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;

        [Header("Movement Settings")]
        public float speed = 5f;
        public float sprintSpeed = 8f;
        public float acceleration = 10f;
        public float deceleration = 10f;
        public float gravity = -15f;

        private Vector3 velocity;
        private Vector3 moveDirection = Vector3.zero;

        [Header("Ground Check")]
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        private bool isGrounded;

        [Header("Sprint Settings")]
        public float sprintDuration = 3f;
        public float sprintCooldown = 2f;
        private float sprintTimer = 0f;
        private bool isSprinting = false;
        private bool canSprint = true;

        [Header("Sprint UI")]
        public Slider sprintBar; // Reference to the UI slider

        void Start()
        {
            if (sprintBar != null)
            {
                sprintBar.maxValue = sprintDuration;
                sprintBar.value = sprintDuration;
            }
        }

        void Update()
        {
            HandleMovement();
            UpdateSprintBar();
        }

        void HandleMovement()
        {
            // Check if grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Get input
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 forward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            Vector3 right = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
            Vector3 targetDirection = (right * x + forward * z).normalized;

            // Sprint Logic
            if (Input.GetKey(KeyCode.LeftShift) && canSprint && targetDirection.magnitude > 0)
            {
                isSprinting = true;
            }

            if (isSprinting)
            {
                sprintTimer += Time.deltaTime;
                if (sprintTimer >= sprintDuration)
                {
                    isSprinting = false;
                    canSprint = false;
                    sprintTimer = sprintDuration;
                    Invoke(nameof(ResetSprint), sprintCooldown);
                }
            }
            else if (!isSprinting && sprintTimer > 0f)
            {
                sprintTimer -= Time.deltaTime;
            }

            float currentSpeed = isSprinting ? sprintSpeed : speed;

            if (targetDirection.magnitude > 0)
            {
                moveDirection = Vector3.Lerp(moveDirection, targetDirection * currentSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
            }

            controller.Move(moveDirection * Time.deltaTime);

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        void UpdateSprintBar()
        {
            if (sprintBar != null)
            {
                sprintBar.value = sprintDuration - sprintTimer; // Update UI
            }
        }

        void ResetSprint()
        {
            canSprint = true;
        }
    }
}
