using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action GameStart;
    public static Action GameEnd;

    private void Awake()
    {
        if (instance == false)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GameStart?.Invoke();
    }

    public void EndGame()
    {
        GameEnd?.Invoke();
    }
}
