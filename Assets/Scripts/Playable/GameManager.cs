using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISetPlayer
{
    [Header("地图生成")]
    public MapChunkGenerator MapChunkGenerator;

    [Header("游戏得分")]
    public int everScoreAppend;
    public ScorePanel ScorePanel;

    public GameOverPanel GameOverPanel;

    private Player player = null;

    public void SetPlayer(GameObject player)
    {
        this.player = player.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MapChunkGenerator.InitializeGenerator();
        ScorePanel.InitializeScorePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        ScorePanel.AddScore(everScoreAppend);
        MapChunkGenerator.GeneratorChunkUpdate();
    }

    public void GameOver()
    {
        player = null;
        string scoreString = ScorePanel.GetScoreString();
        ScorePanel.PanelGo.SetActive(false);
        GameOverPanel.gameObject.SetActive(true);
        GameOverPanel.SetContentText(scoreString);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
