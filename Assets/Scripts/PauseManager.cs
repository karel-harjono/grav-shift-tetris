using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SocialPlatforms.Impl;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign your UI Panel for the menu in Inspector
    public Tilemap tilemap;
    public Board board;

    void Start()
    {
        pauseMenu.SetActive(false); // Hide menu at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // Check if Escape key is pressed
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        if (Time.timeScale == 1f) // If the game is running, pause it
        {
            PauseGame();
        }
        else // If the game is paused, resume it
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze time (pause)
        pauseMenu.SetActive(true); // Show the menu
    }

    public void ResetGame()
    {
        tilemap.ClearAllTiles();
        pauseMenu.SetActive(false); // Hide the Pause Menu at the start
        board.SpawnPiece(); // Spawn the first piece
        ResumeGame(); // Resume the game
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenu.SetActive(false); // Hide the menu
    }
}
