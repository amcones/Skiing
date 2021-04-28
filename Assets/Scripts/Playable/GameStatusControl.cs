using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameStatusControl
{
    public enum GameStatus
    {
        None,
        Gaming,
        Pause,
    }

    public static GameStatusControl CurrentGameStatus
    {
        get
        {
            if (_instanceStatus == null)
                _instanceStatus = new GameStatusControl();
            return _instanceStatus;
        }
    }

    private static GameStatusControl _instanceStatus;

    public GameStatus gameStatus;

    private GameStatusControl()
    {
        gameStatus = GameStatus.None;
    }

    public void Reset()
    {
        Time.timeScale = 1;
        gameStatus = GameStatus.None;
    }

    public void EnterGaming()
    {
        Time.timeScale = 1;
        gameStatus = GameStatus.Gaming;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameStatus = GameStatus.Pause;
    }

    public bool IsGaming()
    {
        return gameStatus == GameStatus.Gaming;
    }

    public bool IsPause()
    {
        return gameStatus == GameStatus.Pause;
    }

    public bool IsNone()
    {
        return gameStatus == GameStatus.None;
    }
}
