using UnityEngine;

namespace SojaExiles
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseXSensitivity = 100f;
        public float mouseYSensitivity = 100f; // Separate sensitivity for Y-axis
        public Transform playerBody;

        float xRotation = 0f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
