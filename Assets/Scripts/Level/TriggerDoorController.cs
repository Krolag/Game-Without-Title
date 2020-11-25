using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Interactable))]
public class TriggerDoorController : MonoBehaviour
{
    public Animator DoorAnimator;

    public bool IsDoorOpen;
    private Interactable _component;

    private void Awake()
    {
        DoorAnimator = GetComponent<Animator>();
        _component = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        _component.Interacted += OnInteracted;
    }

    private void OnDisable()
    {
        _component.Interacted -= OnInteracted;
    }
    private void OnInteracted(Player player)
    {

        if (!IsDoorOpen)
        {
            DoorAnimator.SetTrigger("OpenDoor");
            IsDoorOpen = true;
        }
        else
        {
            DoorAnimator.SetTrigger("CloseDoor");
            IsDoorOpen = false;
        }

    }


}
