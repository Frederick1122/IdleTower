using System;
using Configs;
using TMPro;
using UnityEngine;

public class GameMenuView : UIView<GameMenuModel>
{
    public event Action<UpgradesType> onNewUpgrade; 
    
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private CustomButton _towerDamageButton;
    [SerializeField] private CustomButton _towerCoolDownButton;
    [SerializeField] private CustomButton _enemyCoolDownButton;
    [SerializeField] private CustomButton _costOfEnemyDeathButton;

    public override void Init()
    {
        _towerDamageButton.OnClickButton += () => onNewUpgrade(UpgradesType.TOWER_DAMAGE);
        _towerCoolDownButton.OnClickButton += () => onNewUpgrade(UpgradesType.TOWER_COOLDOWN);
        _enemyCoolDownButton.OnClickButton += () => onNewUpgrade(UpgradesType.ENEMY_COOLDOWN);
        _costOfEnemyDeathButton.OnClickButton += () => onNewUpgrade(UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE);
    }

    public override void UpdateView(GameMenuModel uiModel)
    {
        _coinsText.text = "Coins: " + (int) uiModel.coins;
        _towerDamageButton.SetText($"Damage: {uiModel.towerDamageLevel.value} \n Cost: {uiModel.towerDamageLevel.cost}");
        _towerCoolDownButton.SetText($"Arrow cooldown: {uiModel.towerCoolDownLevel.value} \n Cost: {uiModel.towerCoolDownLevel.cost}");
        _enemyCoolDownButton.SetText($"Enemy cooldown: {uiModel.enemyCoolDownLevel.value} \n Cost: {uiModel.enemyCoolDownLevel.cost}");
        _costOfEnemyDeathButton.SetText($"Cost of enemy death: {uiModel.costOfEnemyDeathLevel.value} \n Cost: {uiModel.costOfEnemyDeathLevel.cost}");
    }
}

public class GameMenuModel : UIModel
{
    public float coins;
    public UpgradeLevel towerDamageLevel;
    public UpgradeLevel towerCoolDownLevel;
    public UpgradeLevel enemyCoolDownLevel;
    public UpgradeLevel costOfEnemyDeathLevel;
}