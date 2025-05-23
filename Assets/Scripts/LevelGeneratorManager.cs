using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelGeneratorManager : MonoBehaviour
{

    public int numberOfLevels = 10;
    public static int currentLevel = 1;

    private void Start()
    {
        GameManager.Instance.OnGameContinue += GameManager_OnGameContinue;
    }

    private void GameManager_OnGameContinue(object sender, EventArgs e)
    {
        if (currentLevel < numberOfLevels)
        {
            // Everytime player wins, increase the level
            currentLevel++;
        }
    }
}

