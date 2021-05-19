using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public virtual void StartRun() { }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.GameOver();
        }
    }
}
