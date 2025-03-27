using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PackageManager : MonoBehaviour
{
    public RectTransform contentRect;
    public ScrollRect chatScrollRect;
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
    private int deliveredPackages = 0; // Tracks successful deliveries
    private float nextPackageDelayMin = 30f; // Minimum delay for next package
    private float nextPackageDelayMax = 60f; // Maximum delay for next package

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
            chatManager.ReceiveMessage($"Unknown: There is a package on the roof of your building. Deliver it to {locationName}. No questions.");
            chatManager.ReceiveMessage($"And be careful. There might be someone after you... ");
            chatManager.AdjustContentSize();
            chatManager.ScrollToBottom();

            // Spawn package at the pickup location
            packagePrefab.transform.position = pickupLocation.position;
            packagePrefab.SetActive(true);

            taskActive = true;
            timeRemaining = deliveryTimeLimit;
            timerActive = true;
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
        deliveredPackages++; // Increment the delivered packages count
        chatManager.ReceiveMessage("You: Package delivered.");
        chatManager.ReceiveMessage("Unknown: Good job. More work soon...");
        chatManager.AdjustContentSize();
        chatManager.ScrollToBottom();

        // Calculate the total required deliveries (5 + number of failed deliveries)
        int requiredDeliveries = 5 + failedDeliveries;

        if (deliveredPackages >= requiredDeliveries)  // If the player has delivered the required number of packages
        {
            chatManager.ReceiveMessage("Unknown: Open the door. Someone is waiting for you.");
            chatManager.AdjustContentSize();
            chatManager.ScrollToBottom();
        }

        hasPackage = false;
        taskActive = false;
        packageIcon.SetActive(false);
        timerActive = false;

        // After the package is delivered, schedule the next package
        StartNextPackageTimer();
    }


    // Handle when the player fails to deliver on time
    public void HandleFailedDelivery()
    {
        timerActive = false;
        failedDeliveries++;
        chatManager.ReceiveMessage($"Unknown: You failed. Now deliver {failedDeliveries} more.");
        chatManager.AdjustContentSize();
        chatManager.ScrollToBottom();
        packageIcon.SetActive(false);

        // Start extra deliveries based on failed attempts
        for (int i = 0; i < failedDeliveries; i++)
        {
            StartNewDelivery();
        }
        hasPackage = false;
        taskActive = false;
        packageIcon.SetActive(false);
        StartNextPackageTimer();
    }

    // Returns the current delivery point so PackageDelivery can check it
    public Transform GetCurrentDeliveryPoint()
    {
        return currentDeliveryPoint;
    }

    // Schedules the next package to spawn between 30 and 60 seconds after a successful delivery
    private void StartNextPackageTimer()
    {
        float delay = Random.Range(nextPackageDelayMin, nextPackageDelayMax); // Random delay between 30 and 60 seconds
        Invoke("StartNewDelivery", delay); // Start the next delivery after the delay
    }
}
