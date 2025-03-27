using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ChatManager : MonoBehaviour
{
    public TextMeshProUGUI messageBox;  // Chat text
    public RectTransform contentRect;  // The Content panel inside Scroll View

    // Notification sound
    public GameObject replyButton;      // Reply button
    public ScrollRect chatScrollRect;   // ScrollRect component

    private int messageIndex = 0;

    private string[] messages = {
        "Unknown: Are you looking for the truth?",
        "Unknown: I know who killed your family.",
        "Unknown: If you want answers, you must prove yourself...",



        "Unknown: Good."
    };

    private string[] replies = {
        "You: Who are you?",
        "You: Tell me everything.",
        "You: What do I need to do?",
        "You: I'll do it...",


    };

    void Start()
    {
        DisplayNextMessage(); // Show first message immediately
        packageManager = FindObjectOfType<PackageManager>();
    }

    public void DisplayNextMessage()
    {
        if (messageIndex < messages.Length)
        {
            messageBox.text += messages[messageIndex] + "\n";
            replyButton.SetActive(true);
            ScrollToBottom();

            if (messageIndex == 2) // If message says "Deliver a package"
            {
                packageManager.StartNewDelivery(); // Start delivery mission
            }
        }
    }
    public void ReceiveMessage(string newMessage)
    {
        messageBox.text += newMessage + "\n"; // Add message to chat
        // Play notification sound
        ScrollToBottom();
    }

    void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases(); // Ensure UI updates before scrolling
        chatScrollRect.verticalNormalizedPosition = 0f; // Scroll to bottom
    }



    public void ReplyToMessage()
    {
        if (messageIndex < replies.Length)
        {
            messageBox.text += replies[messageIndex] + "\n"; // Add reply
            AdjustContentSize(); // Fix height
            replyButton.SetActive(false); // Hide button
            messageIndex++;
            Invoke("DisplayNextMessage", 2f); // Delay next message
            ScrollToBottom();
        }
    }

    void AdjustContentSize()
    {
        Canvas.ForceUpdateCanvases(); // Ensure UI updates before adjusting
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, messageBox.preferredHeight + 20f);
    }

   
    private PackageManager packageManager;
}
