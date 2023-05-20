using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameScript : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject MainMenu;
    public GameObject StartButton;
    public GameObject QuitButton;

    public GameObject ScoreText;

    public GameObject Lives;


    public GameObject PauzeMenu;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        MainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            PauzeMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            PauzeMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        PauzeMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the game + Go back to main menu
    }


    public void StartGame()
    {
        Time.timeScale = 1f;
        MainMenu.SetActive(false);
        StartButton.SetActive(false);
        QuitButton.SetActive(false);
        ScoreText.SetActive(true);
        Lives.SetActive(true);

    }
    public void QuitGame()
    {
        // Quit the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
