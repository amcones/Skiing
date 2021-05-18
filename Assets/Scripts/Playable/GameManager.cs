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
    public GameObject PercentHUDPanel;
    public MistakeHUD MistakeHUDPanel;

    [Header("UI")]
    public GameOverPanel GameOverPanel;
    public GameObject GamePausePanel;
    public GameObject StartView;
    public GameObject StartViewLoading;

    [Header("敌人相关")]
    public EnemyManager EnemyManager;

    [Header("其他")]
    public CameraFollow CameraFollow;
    public Mileage GameMileage;

    private Player player = null;
    private float processScore;
    private bool isPause = false;
    private int startViewStatus = -1;
    private bool canStart = false;

    public void SetPlayer(Player player)
    {
        this.player = player;
        MistakeHUDPanel.SetPlayer(player);
        MapChunkGenerator.SetPlayer(player);
        CameraFollow.SetPlayer(player.gameObject);
        EnemyManager.SetPlayer(player);
        GameMileage.SetPlayer(player);
    }

    // Start is called before the first frame update
    void Start()
    {
        processScore = 0.1f;
        MapChunkGenerator.InitializeGenerator();
        ScorePanel.InitializeScorePanel();
        EnemyManager.InitializeEnemyManager();
        StartViewLoading.SetActive(false);
        startViewStatus = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(startViewStatus == 0)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(WaitForStart(0.5f));
            }
            else
            {
                return;
            }
        }
        if (player == null)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        if (isPause)
            return;

        ScorePanel.AddScore((int)(everScoreAppend * processScore));
        MapChunkGenerator.GeneratorChunkUpdate();
        EnemyManager.GenEnemyUpdate();
        GameMileage.MileageUpdate();
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
        PercentHUDPanel.SetActive(false);
        GameOverPanel.gameObject.SetActive(true);
        GameOverPanel.SetContentText(scoreString);
    }

    public void PauseGame()
    {
        GamePausePanel.SetActive(!GamePausePanel.activeSelf);
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        isPause = !isPause;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool IsPauseGame()
    {
        return isPause;
    }

    public bool CanStart()
    {
        return canStart;
    }

    IEnumerator WaitForStart(float seconds)
    {
        startViewStatus = 1;
        yield return new WaitForSeconds(seconds);
        canStart = true;
        StartView.SetActive(false);
    }
}
