using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public ScorePanel ScorePanel;
    public long overflow;
    public Vector2 GenOffset;
    public float NoticeTime;
    public GameObject NoticIcon;
    public List<GameObject> enemyPrefabs;

    private Player player;
    private List<Enemy> enemies;

    public void SetPlayer(Player player)
    {
        this.player = player;
        foreach(var enemy in enemies)
        {
            enemy.SetPlayer(player);
        }
    }

    public void InitializeEnemyManager()
    {
        enemies = new List<Enemy>();
        NoticIcon.SetActive(false);
        for (int prefabs = 0; prefabs < enemyPrefabs.Count; prefabs++)
        {
            Enemy enemy = GameObject.Instantiate(enemyPrefabs[prefabs]).GetComponent<Enemy>();
            enemy.gameObject.SetActive(false);
            enemies.Add(enemy);
        }
    }

    public void GenEnemyUpdate()
    {
        long factor = ScorePanel.Score % overflow;
        if (factor >= 0 && factor <= 5 && ScorePanel.Score > overflow)
        {
            GenerateEnemy(Random.Range(0, enemies.Count));
        }
    }

    public void GenerateEnemy(int type)
    {
        Enemy enemy = enemies[type];
        if (enemy.gameObject.activeSelf)
            return;

        enemy.gameObject.SetActive(true);
        enemy.transform.position = new Vector2(player.transform.position.x + GenOffset.x, player.transform.position.y + GenOffset.y);
        enemy.StartRun();
        StartCoroutine(NoticeTiming(NoticeTime));
    }

    private IEnumerator NoticeTiming(float time)
    {
        NoticIcon.SetActive(true);
        yield return new WaitForSeconds(time);
        NoticIcon.SetActive(false);
    }
}
