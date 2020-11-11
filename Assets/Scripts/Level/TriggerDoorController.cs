using UnityEngine;
using UnityEngine.UI;

public class TriggerDoorController : MonoBehaviour
{
    public Animator myDoor = null;

    public bool doorTrigger = false;
    public bool isDoorOpen = false;
    
    private void OnTriggerStay(Collider other)
    { 
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.CompareTag("Player"))
            {

                if (doorTrigger)
                {
                    if (!isDoorOpen)
                    {
                        myDoor.Play("openDoor", 0, 0.0f);
                        isDoorOpen = true;
                    }
                    else if (isDoorOpen)
                    {
                        myDoor.Play("closeDoor", 0, 0.0f);
                        isDoorOpen = false;
                    }
                }
            }
        }
    }
}
