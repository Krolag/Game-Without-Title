using System;
using UnityEngine;

[RequireComponent(typeof(Activatable))]
public class Lever : MonoBehaviour
{
    [SerializeField] private Gate _gate;
    private bool _opened;

    private void Awake()
    {
        _opened = false;
    }

    private void OnEnable()
    {
        GetComponent<Activatable>().Activated += OnActivated;
    }

    private void OnDisable()
    {
        GetComponent<Activatable>().Activated -= OnActivated;
    }

    public void OnActivated()
    {
        if (_opened)
        {
            _gate.Close();
            _opened = false;
        }
        else
        {
            _gate.Open();
            _opened = true;
        }
    }

}