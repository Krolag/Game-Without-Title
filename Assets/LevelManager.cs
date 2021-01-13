using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _levelPrefabs;
    [SerializeField] private int _levelIndex = 0;

    public GameObject Player => _player;

    private void Start()
    {
        if (_levelIndex == 0)
        {
            _levelPrefabs[_levelIndex].SetActive(true);
            _player.transform.position = _levelPrefabs[_levelIndex].GetComponent<LevelChanger>().EntryPoint.transform.position;
        }
    }

    public void NextLevel()
    {
        _levelPrefabs[_levelIndex].SetActive(false);
        _levelIndex++;
        _levelPrefabs[_levelIndex].SetActive(true);
        _player.transform.position = _levelPrefabs[_levelIndex].GetComponent<LevelChanger>().EntryPoint.transform.position;
    }

    public void RestartLevel()
    {
        
    }
}
