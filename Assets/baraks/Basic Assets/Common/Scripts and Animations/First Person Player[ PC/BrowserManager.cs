using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BrowserManager : MonoBehaviour
{
    public TextMeshProUGUI urlText;
    public GameObject[] webPages; // Assign different pages in Unity
    private int currentPageIndex = 0;

    void Start()
    {
        ShowPage(0); // Start with the homepage
    }

    public void ShowPage(int index)
    {
        if (index < 0 || index >= webPages.Length) return;

        foreach (GameObject page in webPages)
        {
            page.SetActive(false); // Hide all pages
        }

        webPages[index].SetActive(true); // Show selected page
        currentPageIndex = index;
        urlText.text = "Page: " + webPages[index].name; // Update UI
    }

    public void NextPage()
    {
        if (currentPageIndex < webPages.Length - 1)
            ShowPage(currentPageIndex + 1);
    }

    public void BackPage()
    {
        if (currentPageIndex > 0)
            ShowPage(currentPageIndex - 1);
    }

    public void CloseBrowser()
    {
        gameObject.SetActive(false); // Hide the browser UI
        Cursor.lockState = CursorLockMode.Locked;
    }
}
