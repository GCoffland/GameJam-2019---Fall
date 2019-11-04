﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressSToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            Player.players = new List<Player>();
            SceneManager.LoadScene("TestPlayScene");
        }
        if (Input.GetButtonDown("Start2"))
        {
            Player.players = new List<Player>();
            SceneManager.LoadScene("GamePlayScene");
        }
    }
}
