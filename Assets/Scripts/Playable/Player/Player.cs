using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        Sking,
        TouchBarrier,
        GameOver,
    }

    private static Player currentPlayer;
    public static Player CurrentPlayer => currentPlayer;

    public List<ISetPlayer> Setplayer;
    public string BarriersTag;
    public float SecondAfterBarrier;

    private PlayerMove playerMove;
    private PlayerState playerState;
    // Start is called before the first frame update
    void Start()
    {
        if (Setplayer == null)
            Setplayer = new List<ISetPlayer>();
        playerMove = GetComponent<PlayerMove>();
    }

    public void AddSet(ISetPlayer setPlayer)
    {
        Setplayer.Add(setPlayer);
    }

    public void SetPlayer()
    {
        currentPlayer = this;
        playerMove.enabled = true;
        
        foreach (var set in Setplayer)
        {
            set.SetPlayer(this.gameObject);
        }
    }

    public void PlayerTouchbarrier()
    {
        playerState = PlayerState.TouchBarrier;
        playerMove.TouchBarrier();
    }

    public void PlayerReSking()
    {
        playerState = PlayerState.Sking;
        playerMove.ResetState();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(BarriersTag))
        {
            Debug.Log("Touch");
            if (playerState != PlayerState.TouchBarrier)
            {
                StartCoroutine(WaitForContinueSking(SecondAfterBarrier));
            }
        }
    }

    private IEnumerator WaitForContinueSking(float seconds)
    {
        if (playerState == PlayerState.TouchBarrier || playerState == PlayerState.GameOver)
            yield break;

        PlayerTouchbarrier();
        yield return new WaitForSeconds(seconds);
        PlayerReSking();
    }
}
