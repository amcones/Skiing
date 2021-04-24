using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        Sking,
        TouchBarrier,
        GameOver,
    }

    private static Player currentPlayer = null;
    public static Player CurrentPlayer => currentPlayer;

    public List<ISetPlayer> Setplayer;

    public string BarriersTag;
    public float SecondAfterBarrier;
    public int allowMistakeNumber;

    public UnityEvent mistakeEvent;
    public UnityEvent gameOverEvent;

    private PlayerMove playerMove;
    private PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        if (Setplayer == null)
            Setplayer = new List<ISetPlayer>();
        playerMove = GetComponent<PlayerMove>();
    }

    /// <summary>
    /// ���������ҵ��࣬��ʼ��ʱ����
    /// </summary>
    /// <param name="setPlayer"></param>
    public void AddSet(ISetPlayer setPlayer)
    {
        Setplayer.Add(setPlayer);
    }

    /// <summary>
    /// ���������Ϊ��ǰ��ʵ������ʼ��ʱ����
    /// </summary>
    public void SetPlayer()
    {
        currentPlayer = this;
        playerMove.enabled = true;
        
        foreach (var set in Setplayer)
        {
            set.SetPlayer(this.gameObject);
        }
    }

    /// <summary>
    /// �ж��Ƿ���Ϸ����
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver()
    {
        return playerState == PlayerState.GameOver;
    }

    /// <summary>
    /// �ж��Ƿ���ײ���ϰ���
    /// </summary>
    /// <returns></returns>
    public bool IsTouchBarrier()
    {
        return playerState == PlayerState.TouchBarrier;
    }

    /// <summary>
    /// ��������ʧ��Ĵ���
    /// </summary>
    public void DecreaseAllowMistakeNumber()
    {
        allowMistakeNumber--;
        if(allowMistakeNumber <= 0)
        {
            playerState = PlayerState.GameOver;
            gameOverEvent.Invoke();
        }
    }

    /// <summary>
    /// �����ײ���ϰ������е��¼�
    /// </summary>
    public void PlayerTouchbarrier()
    {
        playerState = PlayerState.TouchBarrier;
        //playerMove.TouchBarrier();
        DecreaseAllowMistakeNumber();
        mistakeEvent.Invoke();
    }

    /// <summary>
    /// ������¿�ʼ����
    /// </summary>
    public void PlayerReSking()
    {
        if (IsGameOver())
            return;

        playerState = PlayerState.Sking;
        playerMove.ResetState();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(BarriersTag))
        {
            if (!IsGameOver() && !IsTouchBarrier())
            {
                StartCoroutine(WaitForContinueSking(SecondAfterBarrier));
            }
        }
    }

    private IEnumerator WaitForContinueSking(float seconds)
    {
        PlayerTouchbarrier();
        yield return new WaitForSeconds(seconds);
        PlayerReSking();
    }
}
