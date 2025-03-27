using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Freezes the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Unfreezes the game
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before changing scenes
        SceneManager.LoadScene("MainMenu");
    }
}
