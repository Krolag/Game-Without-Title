using System;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public event Action Activated; 
    public event Action Deactivated; 
    public void Activate()
    {
        Activated?.Invoke();
    }
    public void Deactivate()
    {
        Deactivated?.Invoke();
    }    
}
