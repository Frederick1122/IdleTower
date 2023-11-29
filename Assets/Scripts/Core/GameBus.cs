using Base;
using System;
using Configs;
using Core;
using Zenject;

public class GameBus : Singleton<GameBus>
{
    public event Action OnUpdateUpgrades;
    public event Action OnUpdateCoins;

    private bool _isContinuedGame;

    private float _coins;
    private int _towerDamageLevel;
    private int _towerCoolDownLevel;
    private int _enemyCoolDownLevel;
    private int _costOfEnemyDeathLevel;

    [Inject] private UpgradesConfig _upgradesConfig;
    
    protected override void Awake()
    {
        base.Awake();
        _coins = BaseDataHandler.GetCoins();
        _towerDamageLevel = BaseDataHandler.GetUpgrade(UpgradesType.TOWER_DAMAGE);
        _towerCoolDownLevel = BaseDataHandler.GetUpgrade(UpgradesType.TOWER_COOLDOWN);
        _enemyCoolDownLevel = BaseDataHandler.GetUpgrade(UpgradesType.ENEMY_COOLDOWN);
        _costOfEnemyDeathLevel = BaseDataHandler.GetUpgrade(UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE);
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
        OnUpdateUpgrades?.Invoke();
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
        OnUpdateCoins?.Invoke();
    }
}