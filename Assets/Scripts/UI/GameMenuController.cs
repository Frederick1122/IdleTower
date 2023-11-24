using System;
using Configs;

public class GameMenuController : UIController<GameMenuView, GameMenuModel>
{
    public override void Init()
    {
        _view.onNewUpgrade += IncrementLevel;
        base.Init();
        
        UpdateView();
    }

    private void OnDestroy()
    {
        if (_view != null)
            _view.onNewUpgrade -= IncrementLevel;
    }

    public override void UpdateView()
    {
        var model = new GameMenuModel()
        {
            coins = GameBus.Instance.GetCoins(),
            towerDamageLevel = GameBus.Instance.GetUpgradeLevel(UpgradesType.TOWER_DAMAGE, true),
            towerCoolDownLevel = GameBus.Instance.GetUpgradeLevel(UpgradesType.TOWER_COOLDOWN, true),
            enemyCoolDownLevel = GameBus.Instance.GetUpgradeLevel(UpgradesType.ENEMY_COOLDOWN, true),
            costOfEnemyDeathLevel = GameBus.Instance.GetUpgradeLevel(UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE, true)
        };
        
        _view.UpdateView(model);
    }

    private void IncrementLevel(UpgradesType type)
    {
        var coins = GameBus.Instance.GetCoins();
        var newUpgradeLevel = GameBus.Instance.GetUpgradeLevel(type, true);
        if (newUpgradeLevel.cost >= coins)
            return;
        
        GameBus.Instance.SetCoins(coins - newUpgradeLevel.cost);
        GameBus.Instance.IncrementLevel(type);
        UpdateView();
    }
}
