using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
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
}
