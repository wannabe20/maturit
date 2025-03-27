using SojaExiles;
using UnityEngine;

public class PCInteract : MonoBehaviour
{
    public GameObject pcUI; // The UI panel for the computer
    public Transform cameraPosition; // Assign a Transform where the player moves to use the PC
    public Transform player; // Assign the Player (not just the camera)

    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private bool isUsingPC = false;

    private PlayerMovement playerMovement; // Reference to movement script
    private MouseLook cameraLook; // Reference to camera movement script

    void Start()
    {
        pcUI.SetActive(false); // Hide UI at start
        playerMovement = player.GetComponent<PlayerMovement>(); // Get PlayerMovement script
        cameraLook = player.GetComponentInChildren<MouseLook>(); // Get CameraLook script
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 2f) // Check if player is close enough
            {
                TogglePC();
            }
        }

        if (isUsingPC && Input.GetKeyDown(KeyCode.Escape)) // Exit PC when pressing Escape
        {
            ExitPC();
        }
    }

    void TogglePC()
    {
        isUsingPC = !isUsingPC;

        if (isUsingPC)
        {
            // Save original player position & rotation
            originalPlayerPosition = player.position;
            originalPlayerRotation = player.rotation;

            // Move player to PC position
            player.position = cameraPosition.position;
            player.rotation = cameraPosition.rotation;

            // Disable player movement & camera movement
            if (playerMovement != null)
                playerMovement.enabled = false;
            if (cameraLook != null)
                cameraLook.enabled = false;

            // Show PC UI
            pcUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ExitPC();
        }
    }

    void ExitPC()
    {
        isUsingPC = false;

        // Move player back to where they were before using the PC
        player.position = originalPlayerPosition;
        player.rotation = originalPlayerRotation;

        // Enable player movement & camera movement again
        if (playerMovement != null)
            playerMovement.enabled = true;
        if (cameraLook != null)
            cameraLook.enabled = true;

        // Hide PC UI
        pcUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
