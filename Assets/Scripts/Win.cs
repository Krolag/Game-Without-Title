using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.name == "MagicalOrb" && Input.GetKeyDown(KeyCode.E) && other.transform.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.name == "Winner" && other.transform.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("ExitScreen", LoadSceneMode.Single);
        }
    }
}
