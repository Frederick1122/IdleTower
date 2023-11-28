using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private Transform _enemyParent;
    [SerializeField] private SphereCollider _sphereCollider;

    private readonly CancellationTokenSource _spawnCts = new();
    private float _spawnCooldown = 3f;
    private float _costOfEnemyDeathLevel;

    private void Start()
    {
        SpawnTask(_spawnCts.Token).Forget();
        UpdateParameters();
        GameBus.Instance.OnUpdateUpgrades += UpdateParameters;
    }

    private void OnDestroy()
    {
        if (GameBus.Instance != null)
            GameBus.Instance.OnUpdateUpgrades -= UpdateParameters;
        
        _spawnCts.Cancel();
    }

    [ContextMenu("Spawn")]
    public void SpawnEnemy()
    {
        var offset = Random.onUnitSphere;
        offset.y = 0;
        var enemyPos = _sphereCollider.transform.position + offset.normalized*_sphereCollider.radius;

        var newEnemy = Instantiate(_enemyPrefab, enemyPos, Quaternion.identity);
        newEnemy.transform.parent = _enemyParent;
        newEnemy.Init(_costOfEnemyDeathLevel);
    }

    private void UpdateParameters()
    {
        _spawnCooldown = GameBus.Instance.GetUpgradeLevel(UpgradesType.ENEMY_COOLDOWN).value;
        _costOfEnemyDeathLevel = GameBus.Instance.GetUpgradeLevel(UpgradesType.COST_OF_ENEMY_DEATH_DAMAGE).value;
    }

    private async UniTaskVoid SpawnTask(CancellationToken token)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnCooldown), cancellationToken: token);
            SpawnEnemy();
        }
    }
}
