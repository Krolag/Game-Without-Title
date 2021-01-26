using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject _backButton;
    public GameObject _quitButton;
    public GameObject _playButton;
    public GameObject _authorsButton;
    public GameObject _authorsPanel;
    public GameObject _descriptionText;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Cursor.visible = true;
        Screen.lockCursor = false;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("newLevel_00", LoadSceneMode.Single);
    }
    public void OnBack()
    {
        _backButton.SetActive(false);
        _authorsPanel.SetActive(false);
        _quitButton.SetActive(true);
        _playButton.SetActive(true);
        _authorsButton.SetActive(true);
        _descriptionText.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnAuthors()
    {
        _backButton.SetActive(true);
        _authorsPanel.SetActive(true);
        _quitButton.SetActive(false);
        _playButton.SetActive(false);
        _authorsButton.SetActive(false);
        _descriptionText.SetActive(false);
    }
}
