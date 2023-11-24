using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "configs/UpgradesConfig", fileName = "UpgradesConfig")]
    public class UpgradesConfig : ScriptableObject
    {
        [SerializeField] private List<UpgradeLevel> _towerDamageLevels = new List<UpgradeLevel>();
        [SerializeField] private List<UpgradeLevel> _towerCoolDownLevels = new List<UpgradeLevel>();
        [SerializeField] private List<UpgradeLevel> _enemyCoolDownLevels = new List<UpgradeLevel>();
        [SerializeField] private List<UpgradeLevel> _costOfEnemyDeathLevels = new List<UpgradeLevel>();

        public UpgradeLevel GetLevelValue(int level, UpgradesType type)
        {
            UpgradeLevel value;
            switch (type)
            {
                case UpgradesType.TOWER_DAMAGE:
                    value = _towerDamageLevels.Count <= level ? _towerDamageLevels[^1] : _towerDamageLevels[level];
                    break;
                case UpgradesType.TOWER_COOLDOWN:
                    value = _towerCoolDownLevels.Count <= level ? _towerCoolDownLevels[^1] : _towerCoolDownLevels[level];
                    break;
                case UpgradesType.ENEMY_COOLDOWN:
                    value = _enemyCoolDownLevels.Count <= level ? _enemyCoolDownLevels[^1] : _enemyCoolDownLevels[level];
                    break;
                case UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE:
                    value = _costOfEnemyDeathLevels.Count <= level ? _costOfEnemyDeathLevels[^1] : _costOfEnemyDeathLevels[level];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return value;
        }
    }

    public enum UpgradesType
    {
        TOWER_DAMAGE,
        TOWER_COOLDOWN,
        ENEMY_COOLDOWN,
        COST_OF_ENEMY_DEATH_DAMAGE
    }

    [Serializable]
    public class UpgradeLevel
    {
        public float value;
        public int cost;
    }
}