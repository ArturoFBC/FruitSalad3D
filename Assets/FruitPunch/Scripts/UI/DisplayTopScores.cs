using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTopScores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myTextBox;

    private void Awake()
    {
        ScoreManager.UpdateScoreEvent += OnScoreUpdated;
    }

    private void OnScoreUpdated(int addedScore, int currentScore)
    {
        string scores = "";
        List<int> scoreBoard = ScoreManager.GetScoreBoard();

        for (int i = 0; i < scoreBoard.Count; i++)
        {
            scores += scoreBoard[i].ToString();
            if (i + 1 < scoreBoard.Count)
                scores += '\n';
        }

        myTextBox.text = scores;
    }

    private void OnDestroy()
    {
        ScoreManager.UpdateScoreEvent -= OnScoreUpdated;
    }
}
