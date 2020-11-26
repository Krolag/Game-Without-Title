using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Interactable))]
public class InteractableOrb : MonoBehaviour
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("ExitScreen", LoadSceneMode.Single);
        }
    }
}
