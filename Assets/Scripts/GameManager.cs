using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject menuButton;

    private int score;
    private int pointsPerLine = 20;
    [SerializeField] private ScoreCounterUI scoreCounter;

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
        this.ResetScore(); // Set the score back to zero
        gameOverPanel.SetActive(false); // Hide Game Over UI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current scene
    }

    public void ExitGame()
    {
        Debug.Log("Game is exiting...");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void IncreaseScore(int numLines)
    {
        // Increase the score by the number of points per line
        switch (numLines)
        {
            case 1:
                score += pointsPerLine;
                break;
            case 2:
                score += pointsPerLine * 4;
                break;
            case 3:
                score += pointsPerLine * 8;
                break;
            case 4:
                score += pointsPerLine * 16;
                break;
        }
        scoreCounter.UpdateScore(score);
    }

    public void ResetScore()
    {
        // Set the score back to zero
        score = 0;
        scoreCounter.UpdateScore(score);
    }
}
