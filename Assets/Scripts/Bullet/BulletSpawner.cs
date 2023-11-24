using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[RequireComponent(typeof(SphereCollider))]
public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _spawnCooldown = 1f;
    [SerializeField] private float _bulletDamage = 1.5f;
    [SerializeField] private float _bulletSpeed = 7f;

    private List<Enemy> _enemies = new List<Enemy>();
    private Coroutine _spawnRoutine;
    
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

    private void UpdateEnemiesStack()
    {
        if (_enemies.Count > 0 && _spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnBulletRoutine());
        else if (_enemies.Count == 0 && _spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
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
    
    private IEnumerator SpawnBulletRoutine()
    {
        while (_enemies.Count > 0)
        {
          SpawnBullet(_enemies[^1]);
          yield return new WaitForSeconds(_spawnCooldown);
        }

        _spawnRoutine = null;
        UpdateEnemiesStack();
    }
}
