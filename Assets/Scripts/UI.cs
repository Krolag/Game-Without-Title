using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("newLevel_00", LoadSceneMode.Single);
    }
}
