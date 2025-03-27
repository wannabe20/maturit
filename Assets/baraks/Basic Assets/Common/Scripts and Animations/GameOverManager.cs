using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // UI panel pro Game Over

    void Start()
    {
        Time.timeScale = 1f; //Ujistíme se, že hra po restartu bìží normálnì
        gameOverPanel.SetActive(false); // Skryjeme Game Over menu
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pauzneme hru
        Cursor.lockState = CursorLockMode.None; // Odemkneme kurzor
        Cursor.visible = true; // Zviditelníme kurzor
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart scény
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Naèteme hlavní menu
    }
}
