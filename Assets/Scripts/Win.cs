using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Interactable))]
public class Win : MonoBehaviour
{
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        interactable.Interacted += OnInteracted;
    }

    private void OnDisable()
    {
        interactable.Interacted -= OnInteracted;
    }

    private void OnInteracted(Player player)
    {
        if (this.gameObject.name == "MagicalOrb")
        {
            Destroy(this.gameObject);
        }
    }

    /* private void OnTriggerEnter(Collider other)
     {
         if (this.gameObject.name == "Winner" && other.transform.CompareTag("Player"))
         {
             Cursor.lockState = CursorLockMode.None;
             Cursor.visible = true;
             SceneManager.LoadScene("ExitScreen", LoadSceneMode.Single);
         }
     }*/
}
