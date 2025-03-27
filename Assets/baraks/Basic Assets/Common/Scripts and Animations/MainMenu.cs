using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;  // Assign the Main Menu Canvas
    public GameObject settingsCanvas;  // Assign the Settings Canvas

    void Start()
    {
        GameObject mainMenuUI = GameObject.Find("MainMenuCanvas"); // Change to your UI GameObject name
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        mainMenuCanvas.SetActive(false);  // Hide Main Menu
        settingsCanvas.SetActive(true);   // Show Settings
    }

    public void BackToMainMenu()
    {
        settingsCanvas.SetActive(false);  // Hide Settings
        mainMenuCanvas.SetActive(true);   // Show Main Menu
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting..."); // For testing in the editor
    }


}
