using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject MainMenu;
    public GameObject StartButton;
    public GameObject QuitButton;

    public GameObject ScoreText;

    public GameObject Lives;

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
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
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
