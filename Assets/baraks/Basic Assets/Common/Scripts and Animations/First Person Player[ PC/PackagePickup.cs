using UnityEngine;

public class PackagePickup : MonoBehaviour
{
    private PackageManager packageManager;
    private Transform player;
    public float interactDistance = 2f; // Distance for interaction

    private bool hasPackage = false;

    void Start()
    {
        packageManager = FindObjectOfType<PackageManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get the player
    }

    void Update()
    {
        if (!hasPackage && Vector3.Distance(transform.position, player.position) < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            PickupPackage();
        }
    }

    void PickupPackage()
    {
        hasPackage = true;
        gameObject.SetActive(false); // Hide the package object
        packageManager.packageIcon.SetActive(true); // Show package icon
        packageManager.chatManager.ReceiveMessage("You: I have the package. Delivering now.");
        packageManager.SetHasPackage(true); // Let PackageManager know player has a package
        Debug.Log("Package picked up!");
    }
}
