using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // UI panel pro Game Over

    void Start()
    {
        Time.timeScale = 1f; //Ujist�me se, �e hra po restartu b�� norm�ln�
        gameOverPanel.SetActive(false); // Skryjeme Game Over menu
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pauzneme hru
        Cursor.lockState = CursorLockMode.None; // Odemkneme kurzor
        Cursor.visible = true; // Zviditeln�me kurzor
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart sc�ny
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Na�teme hlavn� menu
    }
}
