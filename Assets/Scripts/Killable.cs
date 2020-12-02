using System;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public event Action Death;

    public void Die()
    {
        Death?.Invoke();
    }
}