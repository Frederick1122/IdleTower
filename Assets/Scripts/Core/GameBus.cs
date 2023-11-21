using Base;
using System;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    public event Action OnRestartGame;
    public event Action OnContinueGame;
    public event Action OnEndGame;

    [SerializeField] private Tower _tower;
    
    private bool _isContinuedGame;

    public Tower GetTower() => _tower;

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void RestartGame()
    {
        OnRestartGame?.Invoke();
        _isContinuedGame = false;
    }

    public void ContinueGame()
    {
        OnContinueGame?.Invoke();
        _isContinuedGame = true;
    }

    public bool IsContinuedGame()
    {
        return _isContinuedGame;
    }

}
