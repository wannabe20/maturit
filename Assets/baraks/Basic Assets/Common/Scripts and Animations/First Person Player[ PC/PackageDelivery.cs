using UnityEngine;

public class PackageDelivery : MonoBehaviour
{
    private PackageManager packageManager;
    private Transform player;
    public GameObject packageAtDelivery; // Assign the pre-placed package in the inspector
    public float interactDistance = 2f; // Distance required to deliver
    public float despawnTime = 30f; // Time before the delivered package disappears

    private bool isCorrectDeliverySpot = false;

    void Start()
    {
        packageManager = FindObjectOfType<PackageManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get player reference

        // Make sure the package is hidden at the start
        if (packageAtDelivery != null)
        {
            packageAtDelivery.SetActive(false);
        }
    }

    void Update()
    {
        if (isCorrectDeliverySpot && packageManager.HasPackage() &&
            Vector3.Distance(transform.position, player.position) < interactDistance &&
            Input.GetKeyDown(KeyCode.E))
        {
            DeliverPackage();
        }
    }

    void DeliverPackage()
    {
        packageManager.PackageDelivered();
        packageManager.packageIcon.SetActive(false); // Hide UI icon

        // Enable the pre-placed package
        if (packageAtDelivery != null)
        {
            packageAtDelivery.SetActive(true);
            Debug.Log("Package delivered at " + transform.position);

            // Schedule package disappearance
            Invoke(nameof(DespawnDeliveredPackage), despawnTime);
        }
    }

    void DespawnDeliveredPackage()
    {
        if (packageAtDelivery != null)
        {
            packageAtDelivery.SetActive(false);
            Debug.Log("Delivered package removed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform == packageManager.GetCurrentDeliveryPoint())
        {
            isCorrectDeliverySpot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCorrectDeliverySpot = false;
        }
    }
}
