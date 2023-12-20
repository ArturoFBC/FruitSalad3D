using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;

    void Awake()
    {
        if (myText == null)
            myText = GetComponent<TextMeshProUGUI>();

        ScoreManager.UpdateScoreEvent += OnScoreUpdated;

        OnScoreUpdated(0, ScoreManager.GetScore());
    }

    private void OnDestroy()
    {
        ScoreManager.UpdateScoreEvent -= OnScoreUpdated;
    }

    private void OnScoreUpdated(int addedScore, int currentScore)
    {
        myText.text = currentScore.ToString();
    }
}
