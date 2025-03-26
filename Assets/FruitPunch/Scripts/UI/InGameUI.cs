using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.GameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        GameManager.GameEnd -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        gameOverScreen.SetActive(true);
    }
}
