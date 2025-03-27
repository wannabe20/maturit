using UnityEngine;
using TMPro;

public class PackageManager : MonoBehaviour
{
    public GameObject packagePrefab; // Assign the Package object in Unity
    public Transform pickupLocation; // Where the package spawns
    public Transform[] deliveryLocations; // Different possible delivery spots
    public GameObject packageIcon; // UI icon for carrying a package
    public float deliveryTimeLimit = 300f; // Time in seconds to deliver
    public ChatManager chatManager; // Reference to the chat system

    private Transform currentDeliveryPoint;
    private bool hasPackage = false;
    private bool taskActive = false;
    private float timeRemaining;
    private bool timerActive = false;
    private int failedDeliveries = 0; // Tracks failed attempts

    void Start()
    {
        packagePrefab.SetActive(false); // Hide package at start
        packageIcon.SetActive(false);   // Hide UI package icon at start
    }

    void Update()
    {
        if (timerActive)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                HandleFailedDelivery();
            }
        }
    }

    // Start a new delivery task
    public void StartNewDelivery()
    {
        if (!taskActive)
        {
            // Choose a random delivery location
            currentDeliveryPoint = deliveryLocations[Random.Range(0, deliveryLocations.Length)];

            // Get the name of the delivery location dynamically
            string locationName = currentDeliveryPoint.name.Replace("DeliveryPoint_", "Apt ");

            // Unknown assigns a new delivery task in chat
            chatManager.ReceiveMessage($"Unknown: Deliver a package to {locationName}. No questions.");

            // Spawn package at the pickup location
            packagePrefab.transform.position = pickupLocation.position;
            packagePrefab.SetActive(true);

            taskActive = true;
        }
    }

    // Called by PackagePickup when the player picks up the package
    public void SetHasPackage(bool value)
    {
        hasPackage = value;

        if (value)
        {
            packageIcon.SetActive(true); // Show package icon in UI
            timeRemaining = deliveryTimeLimit;
            timerActive = true;
        }
        else
        {
            packageIcon.SetActive(false); // Hide package icon when delivered
            timerActive = false;
        }
    }

    public bool HasPackage()
    {
        return hasPackage;
    }

    // Called by PackageDelivery when the package is delivered
    public void PackageDelivered()
    {
        chatManager.ReceiveMessage("You: Package delivered.");
        chatManager.ReceiveMessage("Unknown: Good job. More work soon...");

        hasPackage = false;
        taskActive = false;
        packageIcon.SetActive(false);
        timerActive = false;
    }

    // Handle when the player fails to deliver on time
    public void HandleFailedDelivery()
    {
        timerActive = false;
        failedDeliveries++;
        chatManager.ReceiveMessage($"Unknown: You failed. Now deliver {failedDeliveries} more.");
        packageIcon.SetActive(false);

        // Start extra deliveries based on failed attempts
        for (int i = 0; i < failedDeliveries; i++)
        {
            StartNewDelivery();
        }
    }

    // Returns the current delivery point so PackageDelivery can check it
    public Transform GetCurrentDeliveryPoint()
    {
        return currentDeliveryPoint;
    }
}
