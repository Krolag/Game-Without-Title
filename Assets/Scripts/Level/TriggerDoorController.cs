using System;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDoorController : MonoBehaviour
{
    public Animator DoorAnimator;

    public bool IsDoorOpen;

    private void Start()
    {
        DoorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (!IsDoorOpen)
            {
                DoorAnimator.SetBool("OpenDoor", true);
                DoorAnimator.SetBool("CloseDoor", false);
                IsDoorOpen = true;
            }
            else
            {
                            DoorAnimator.SetBool("OpenDoor", false);
                DoorAnimator.SetBool("CloseDoor", true);
                IsDoorOpen = false;
            }
        }
    }
}
