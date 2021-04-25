using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("地图生成")]
    public MapChunkGenerator MapChunkGenerator;

    [Header("游戏得分")]
    public int everScoreAppend;
    public float gradualScoreRate;
    public ScorePanel ScorePanel;

    [Header("HUD")]
    public MistakeHUD MistakeHUDPanel;
    public GameOverPanel GameOverPanel;

    [Header("敌人相关")]
    public EnemyManager EnemyManager;

    [Header("其他")]
    public CameraFollow CameraFollow;
    
    private Player player = null;
    private float processScore;

    public void SetPlayer(Player player)
    {
        this.player = player;
        MistakeHUDPanel.InitializeMistakePanel(this.player.maxAllowMistakeNumber);
        MapChunkGenerator.SetPlayer(player);
        CameraFollow.SetPlayer(player.gameObject);
        EnemyManager.SetPlayer(player);
    }

    // Start is called before the first frame update
    void Start()
    {
        processScore = 0.1f;
        MapChunkGenerator.InitializeGenerator();
        ScorePanel.InitializeScorePanel();
        EnemyManager.InitializeEnemyManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        ScorePanel.AddScore((int)(everScoreAppend * processScore));
        MapChunkGenerator.GeneratorChunkUpdate();
        EnemyManager.GenEnemyUpdate();
        if(processScore < 1.0f)
        {
            processScore += gradualScoreRate;
        }
    }

    public void GameOver()
    {
        player = null;
        string scoreString = ScorePanel.GetScoreString();
        ScorePanel.PanelGo.SetActive(false);
        MistakeHUDPanel.gameObject.SetActive(false);
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
