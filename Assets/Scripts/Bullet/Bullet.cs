using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private EnemyBody _enemyBody;
    
    private void OnTriggerEnter(Collider other)
    {
        var enemyBody = other.GetComponent<EnemyBody>();
        if (enemyBody == null || enemyBody != _enemyBody)
            return;
        
        enemyBody.GetEnemy().AddDamage(_damage);
        Destroy(gameObject);
    }

    private void Update()
    {
        var a = transform.forward;
        
        transform.Translate( transform.up  * _speed * Time.deltaTime);
    }

    public void Init(float damage, float speed, EnemyBody enemyBody)
    {
        _damage = damage;
        _speed = speed;
        _enemyBody = enemyBody;
    }
}
