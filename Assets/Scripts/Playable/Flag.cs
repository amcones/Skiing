using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public int addScore = 150;
    public ScorePanel score;

    Transform playerTransform;
    bool isReach = true;

    public bool IsReach { get => isReach; }

    public void SetPlay(Player player)
    {
        playerTransform = player.transform;
        isReach = true;
    }

    public void Initialize(float y)
    {
        if (!isReach)
            return;
        transform.position = new Vector2(transform.position.x, y);
        isReach = false;
    }

    void Update()
    {
        if (!isReach)
        {
            transform.position = new Vector2(playerTransform.position.x, transform.position.y);
            if (playerTransform.position.y <= transform.position.y && !isReach)
            {
                score.AddScore(150);
                isReach = true;
            }
        }
    }
}
