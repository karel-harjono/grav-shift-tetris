using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class ControlsMenu : MonoBehaviour
{
    public GameObject startPanel;    
    public GameObject controlsPanel; 
    public Button startButton;       
    public Button backButton;        
    public GameObject menuButton;
    public Tilemap tilemap; // Reference to the Tilemap
    public Board board;

    private bool gameStarted = false; // To prevent multiple restarts

    void Start()
    {
        // Initially, show Start Panel and hide Controls Panel
        startPanel.SetActive(true);
        controlsPanel.SetActive(false);
        menuButton.SetActive(false);

        // Assign button click listeners
        startButton.onClick.AddListener(OpenControls);
        backButton.onClick.AddListener(CloseControls);
    }

    void Update()
    {
        // If in Controls Menu and a valid key is pressed, start the game
        if (!gameStarted && controlsPanel.activeSelf && AnyKeyboardKeyPressed())
        {
            StartGame();
        }
    }

    bool AnyKeyboardKeyPressed()
    {
        // Check if any key is pressed, excluding mouse buttons & UI clicks
        return Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2);
    }

    void StartGame()
    {
        gameStarted = true; // Prevent multiple starts
        controlsPanel.SetActive(false); // Hide Controls Menu
        menuButton.SetActive(true); // Show Menu Button
        tilemap.ClearAllTiles(); // Clear all tiles
        board.SpawnPiece(); // Spawn the first piece
    }

    void OpenControls()
    {
        startPanel.SetActive(false);  
        controlsPanel.SetActive(true); 
    }

    void CloseControls()
    {
        controlsPanel.SetActive(false);
        startPanel.SetActive(true);    
    }
}
