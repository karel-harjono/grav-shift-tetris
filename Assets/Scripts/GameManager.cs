using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject menuButton;

    void Start()
    {
        gameOverPanel.SetActive(false); // Hide Game Over panel at start
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        if (menuButton != null) menuButton.SetActive(false); // Hide menu button
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false); // Hide Game Over UI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current scene
    }

    public void ExitGame()
    {
        Debug.Log("Game is exiting...");
        UnityEditor.EditorApplication.isPlaying = false;
        // Application.Quit();
    }
}
