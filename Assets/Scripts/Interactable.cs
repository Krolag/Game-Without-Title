using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public event Action<Player> Interacted;
    public event Action StopedInteraction;
    public void Interact(Player player)
    {
        Interacted?.Invoke(player);
    }

    public void StopInteraction()
    {
        StopedInteraction?.Invoke();
    }
}
