using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        Skiing,
        TouchBarrier,
        GameOver,
    }

    private static Player currentPlayer = null;
    public static Player CurrentPlayer => currentPlayer;

    public string BarriersTag;
    public float SecondAfterBarrier;
    public int maxAllowMistakeNumber;

    public UnityEvent mistakeEvent;
    public UnityEvent gameOverEvent;

    private PlayerMove playerMove;
    private PlayerState playerState;
    private int allowMistakeNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        allowMistakeNumber = maxAllowMistakeNumber;
    }
    /// <summary>
    /// 将玩家设置为当前的实例，初始化时调用
    /// </summary>
    public void SetPlayer()
    {
        currentPlayer = this;
        playerMove.enabled = true;
    }

    public float GetCurrentSpeedUpTime()
    {
        return playerMove.GetSpeedUpTime();
    }

    public float GetSpeedUpWaitTime()
    {
        return playerMove.waitSpeedUpTime;
    }

    public void AddSpeedUpEvent(UnityAction action)
    {
        playerMove.addSpeedUpEvent.AddListener(action);
    }

    /// <summary>
    /// 判断是否游戏结束
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver()
    {
        return playerState == PlayerState.GameOver;
    }

    /// <summary>
    /// 判断是否碰撞到障碍物
    /// </summary>
    /// <returns></returns>
    public bool IsTouchBarrier()
    {
        return playerState == PlayerState.TouchBarrier;
    }

    public int GetAllowMistakeNumber()
    {
        return allowMistakeNumber;
    }

    /// <summary>
    /// 增加允许失误的次数
    /// </summary>
    public void AddAllowMistakeNumber()
    {
        allowMistakeNumber = Mathf.Clamp(allowMistakeNumber + 1, 0, maxAllowMistakeNumber);
    }

    /// <summary>
    /// 减少允许失误的次数
    /// </summary>
    public void DecreaseAllowMistakeNumber()
    {
        allowMistakeNumber--;
        if(allowMistakeNumber <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 玩家碰撞到障碍物运行的事件
    /// </summary>
    public void PlayerTouchbarrier()
    {
        playerState = PlayerState.TouchBarrier;
        DecreaseAllowMistakeNumber();
        mistakeEvent.Invoke();
    }

    public void GameOver()
    {
        playerState = PlayerState.GameOver;
        playerMove.StopMove();
        gameOverEvent.Invoke();
    }

    /// <summary>
    /// 玩家重新开始滑冰
    /// </summary>
    public void PlayerReSking()
    {
        if (IsGameOver())
            return;

        playerState = PlayerState.Skiing;
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
