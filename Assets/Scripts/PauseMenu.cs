using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;
    public GameObject _pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Screen.lockCursor = true;
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Screen.lockCursor = false;
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Cursor.visible = true;
        Screen.lockCursor = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
