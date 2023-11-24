using Base;
using System;
using Configs;
using Core;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    public event Action OnRestartGame;
    public event Action OnContinueGame;
    public event Action OnEndGame;

    [SerializeField] private Tower _tower;
    [SerializeField] private UpgradesConfig _upgradesConfig;
    
    private bool _isContinuedGame;

    private float _coins;
    private int _towerDamageLevel;
    private int _towerCoolDownLevel;
    private int _enemyCoolDownLevel;
    private int _costOfEnemyDeathLevel;

    protected override void Awake()
    {
        base.Awake();
        _coins = BaseDataHandler.GetCoins();
        _towerDamageLevel = BaseDataHandler.GetUpgrade(UpgradesType.TOWER_DAMAGE);
        _towerCoolDownLevel = BaseDataHandler.GetUpgrade(UpgradesType.TOWER_COOLDOWN);
        _enemyCoolDownLevel = BaseDataHandler.GetUpgrade(UpgradesType.ENEMY_COOLDOWN);
        _costOfEnemyDeathLevel = BaseDataHandler.GetUpgrade(UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE);
    }

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

    public void IncrementLevel(UpgradesType type)
    {
        var level = 0; 
        switch (type)
        {
            case UpgradesType.TOWER_DAMAGE:
                level = ++_towerDamageLevel;
                break;
            case UpgradesType.TOWER_COOLDOWN:
                level = ++_towerCoolDownLevel;
                break;
            case UpgradesType.ENEMY_COOLDOWN:
                level = ++_enemyCoolDownLevel;
                break;
            case UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE:
                level = ++_costOfEnemyDeathLevel;
                break;
        }
        
        BaseDataHandler.SaveUpgrade(level, type);
    }

    public UpgradeLevel GetUpgradeLevel(UpgradesType type, bool nextLevel = false)
    {
        var level = type switch
        {
            UpgradesType.TOWER_DAMAGE => _towerDamageLevel,
            UpgradesType.TOWER_COOLDOWN => _towerCoolDownLevel,
            UpgradesType.ENEMY_COOLDOWN => _enemyCoolDownLevel,
            UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE => _costOfEnemyDeathLevel,
            _ => 0
        };
        
        level = nextLevel ? level + 1 : level;
        
        return _upgradesConfig.GetLevelValue(level, type);
    }

    public float GetCoins()
    {
        return _coins;
    }

    public void SetCoins(float coins)
    {
        _coins = coins;
        BaseDataHandler.SaveCoins(_coins);
    }
}