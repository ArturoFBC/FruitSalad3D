using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;

    public static Action<int,int> UpdateScoreEvent;

    private void Awake()
    {
        GameManager.GameStart   += OnGameStart;
        GameManager.GameEnd     += OnGameEnd;
    }

    private void OnDestroy()
    {
        GameManager.GameStart -= OnGameStart;
        GameManager.GameEnd -= OnGameEnd;
    }

    private void OnGameStart()
    {
        score = 0;
        PieceBahaviour.PieceCombinationEvent += OnPieceCombination;
    }

    private void OnGameEnd()
    {
        PieceBahaviour.PieceCombinationEvent -= OnPieceCombination;
    }

    private void OnPieceCombination(int newSize)
    {
        int addedScore = newSize * 10;
        score += addedScore;

        UpdateScoreEvent?.Invoke(addedScore, score);
    }
}
