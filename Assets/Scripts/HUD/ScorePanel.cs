using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public GameObject PanelGo;
    public Text ScoreText;
    public string ScoreColor = "red";
    private ScoreData score;

    public void InitializeScorePanel()
    {
        score = new ScoreData();
    }

    public void AddScore(int score)
    {
        this.score.AddScore(score);
        UpdateScorePanel();
    }

    public void DecreaseScore(int score)
    {
        this.score.DecreaseScore(score);
        UpdateScorePanel();
    }

    public string GetScoreString()
    {
        return $"Score: <color=\"{ScoreColor}\">{score.Score}</color>";
    }

    public void UpdateScorePanel()
    {
        ScoreText.text = GetScoreString();
    }
}
