using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject pauseScreen;
    public GameObject startScreen;
    public GameObject playerHUD;

    public MovementScript player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.playerWins)
        {
            winScreen.SetActive(true);
            player.canDie = false;
            Time.timeScale = 0f;
        }
        if (player.playerLoses)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
            playerHUD.SetActive(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        playerHUD.SetActive(false);
        player.isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        playerHUD.SetActive(true);
        player.isPaused = false;
    }

    public void RedoLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        print("quit game :D");
    }
}
