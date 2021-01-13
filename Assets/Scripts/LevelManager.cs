using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _levelPrefabs;
    [SerializeField] private int _levelIndex = 0;
    [Header("Listeners")]
    [SerializeField] private EventObject _noiseListener;

    public GameObject Player => _player;

    private void Start()
    {
        _levelPrefabs[_levelIndex].SetActive(true);
        _player.transform.position = _levelPrefabs[_levelIndex].GetComponent<LevelChanger>().EntryPoint.position;
        ResetAllListeners();
    }

    public void NextLevel()
    {
        _levelPrefabs[_levelIndex].SetActive(false);
        _levelIndex++;
        ResetAllListeners();
        _player.transform.position = _levelPrefabs[_levelIndex].GetComponent<LevelChanger>().EntryPoint.position;
        _levelPrefabs[_levelIndex].SetActive(true);
    }

    public void ResetAllListeners()
    {
        _noiseListener.RemoveAllListeners();
    }

    public void RestartLevel()
    {
        
    }
}
