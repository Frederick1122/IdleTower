using System;
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
    [SerializeField] private float _bulletSpeed = 3f;

    private List<Enemy> _enemies = new List<Enemy>();
    private Coroutine _spawnRoutine;
    
    private void OnTriggerEnter(Collider other)
    {
        var enemyBody = other.GetComponent<EnemyBody>();
        if (enemyBody != null)
        {
            var enemy = enemyBody.GetEnemy();
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
            StopCoroutine(_spawnRoutine);
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
        var direction = enemy.GetEnemyBody().transform.position - bullet.transform.position;
        // var angle = Math.Atan2(bullet.transform.forward.y - direction.y, bullet.transform.forward.x - direction.x);
        var angleY = Vector3.SignedAngle(Utils.GetWithoutAxis(bullet.transform.forward, Enums.Axis.y), 
            Utils.GetWithoutAxis(direction, Enums.Axis.y), Vector3.up);
        
        var angleX = Vector3.SignedAngle(Utils.GetWithoutAxis(bullet.transform.up, Enums.Axis.x), 
            Utils.GetWithoutAxis(direction, Enums.Axis.x), Vector3.right);
        var euler = bullet.transform.eulerAngles;
        euler.y = angleY;
        euler.x = angleX;
        bullet.transform.eulerAngles = euler;

        bullet.transform.parent = transform;
        bullet.Init(_bulletDamage, _bulletSpeed, enemy.GetEnemyBody());
    }
    

    private IEnumerator SpawnBulletRoutine()
    {
        while (_enemies.Count > 0)
        {
          SpawnBullet(_enemies[^1]);
          yield return new WaitForSeconds(_spawnCooldown);
        }
        
        UpdateEnemiesStack();
    }
}
