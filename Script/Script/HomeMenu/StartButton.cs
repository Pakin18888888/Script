using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;
    
    void Start()
    {
        startButton.SetActive(false);
    }

    public void ShowButton()
    {
        startButton.SetActive(true);
    }

    
}

