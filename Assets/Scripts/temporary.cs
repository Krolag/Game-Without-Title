using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class temporary : MonoBehaviour
{
    public TextMeshProUGUI Objectives;
    public GameObject Orb;
    public static int EnemiesLeft = 12;
    
    // Update is called once per frame
    void Update()
    {
        if (EnemiesLeft > 0)
            Objectives.text = "Enemies left: " + EnemiesLeft;
    }
}
