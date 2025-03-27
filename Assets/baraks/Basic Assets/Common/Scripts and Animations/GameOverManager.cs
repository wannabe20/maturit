using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // UI panel for Game Over

    void Start()
    {
        Time.timeScale = 1f; // Ensure the game runs normally after a restart
        gameOverPanel.SetActive(false); // Hide Game Over panel initially
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene (restart the game)
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
        
    }
}
