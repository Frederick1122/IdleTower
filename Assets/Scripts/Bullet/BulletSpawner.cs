using System;
using System.Collections.Generic;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed = 7f;

    private float _spawnCooldown = 1f;
    private float _bulletDamage = 1.5f;
    private List<Enemy> _enemies = new();
    private CancellationTokenSource _spawnCts;
    
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _enemies.Add(enemy);
            enemy.OnDie += RemoveEnemy; 
            UpdateEnemiesStack();
        }
    }

    private void Start()
    {
        GameBus.Instance.OnUpdateUpgrades += UpdateParameters;
        UpdateParameters();
    }

    private void OnDestroy()
    {
        if (GameBus.Instance != null)
            GameBus.Instance.OnUpdateUpgrades -= UpdateParameters;
    }

    private void UpdateEnemiesStack()
    {
        if (_enemies.Count > 0 && (_spawnCts == null || _spawnCts.IsCancellationRequested))
        {
            _spawnCts = new CancellationTokenSource();
            SpawnBulletTask(_spawnCts).Forget();
        }
        else if (_enemies.Count == 0 && !_spawnCts.IsCancellationRequested) 
            _spawnCts.Cancel();
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemy.OnDie -= RemoveEnemy;
        _enemies.Remove(enemy);
        UpdateEnemiesStack();
    }

    private void SpawnBullet(Enemy enemy)
    {
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.LookAt(enemy.GetTargetPoint().position);
        bullet.transform.parent = transform;
        bullet.Init(_bulletDamage, _bulletSpeed, enemy);
    }

    private void UpdateParameters()
    {
        _bulletDamage = GameBus.Instance.GetUpgradeLevel(UpgradesType.TOWER_DAMAGE).value;
        _spawnCooldown = GameBus.Instance.GetUpgradeLevel(UpgradesType.TOWER_DAMAGE).value;
    }
    
    private async UniTaskVoid SpawnBulletTask(CancellationTokenSource cts)
    {
        while (_enemies.Count > 0)
        {
          SpawnBullet(_enemies[0]);
          await UniTask.Delay(TimeSpan.FromSeconds(_spawnCooldown), cancellationToken: cts.Token);
        }

        cts.Cancel();
        UpdateEnemiesStack();
    }
}
