using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player currentPlayer;
    public static Player CurrentPlayer => currentPlayer;

    public List<ISetPlayer> Setplayer;

    private PlayerMove PlayerMove;

    // Start is called before the first frame update
    void Start()
    {
        if (Setplayer == null)
            Setplayer = new List<ISetPlayer>();
        PlayerMove = GetComponent<PlayerMove>();
    }

    public void AddSet(ISetPlayer setPlayer)
    {
        Setplayer.Add(setPlayer);
    }

    public void SetPlayer()
    {
        currentPlayer = this;
        PlayerMove.enabled = true;
        
        foreach (var set in Setplayer)
        {
            set.SetPlayer(this.gameObject);
        }
    }
}
