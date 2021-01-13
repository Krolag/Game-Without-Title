using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private Transform _entryPoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private LevelManager _levelManager;

    public Transform EntryPoint => _entryPoint;
    public Transform ExitPoint => _exitPoint;

    private void Update()
    {
        if (Vector3.Distance(_levelManager.Player.transform.position, _exitPoint.position) < 5)
            _levelManager.NextLevel();
    }
}
