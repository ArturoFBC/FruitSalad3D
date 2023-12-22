using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const int SCORE_BOARD_LENGTH = 5;
    private const string SCORE_PLAYER_PREFS_KEY = "score_";

    private static int score;

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
        UpdateScoreEvent?.Invoke(0, score);
        PieceBahaviour.PieceCombinationEvent += OnPieceCombination;
    }

    private void OnGameEnd()
    {
        PieceBahaviour.PieceCombinationEvent -= OnPieceCombination;
        SaveCurrentScore();
        UpdateScoreEvent?.Invoke(0, score);
    }

    private void OnPieceCombination(int newSize)
    {
        int addedScore = newSize * 10;
        score += addedScore;

        UpdateScoreEvent?.Invoke(addedScore, score);
    }

    public static List<int> GetScoreBoard()
    {
        List<int> scoreBoard = new List<int>();
        for (int scoreIndex = 0; scoreIndex < SCORE_BOARD_LENGTH; scoreIndex++)
        {
            string currentScoreKey = SCORE_PLAYER_PREFS_KEY + scoreIndex.ToString();
            if (PlayerPrefs.HasKey(currentScoreKey))
                scoreBoard.Add(PlayerPrefs.GetInt(currentScoreKey));
            else
                scoreBoard.Add(0);
        }

        return scoreBoard;
    }

    private void SaveCurrentScore()
    {
        int scoreToSave = score;
        for (int scoreIndex = 0; scoreIndex < SCORE_BOARD_LENGTH; scoreIndex++)
        {
            string currentScoreKey = SCORE_PLAYER_PREFS_KEY + scoreIndex.ToString();
            if (PlayerPrefs.HasKey(currentScoreKey))
            {
                int currentScore = PlayerPrefs.GetInt(currentScoreKey);

                if (currentScore < scoreToSave)
                {
                    PlayerPrefs.SetInt(currentScoreKey, scoreToSave);
                    scoreToSave = currentScore;
                }
            }
            else
            {
                if (scoreToSave > 0)
                    PlayerPrefs.SetInt(currentScoreKey, scoreToSave);

                break;
            }
        }
    }

    public static int GetScore()
    {
        return score;
    }
}
