using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";
    
    private float _speed = 1;
    private float _damage;
    private EnemyBody _enemyBody;
    
    private void OnTriggerEnter(Collider other)
    {
        var enemyBody = other.GetComponent<EnemyBody>();
        if (enemyBody != null && enemyBody == _enemyBody)
        {
            enemyBody.GetEnemy().AddDamage(_damage);
            Destroy(gameObject);
        }

        if (other.CompareTag(GROUND_TAG))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate( Vector3.forward  * _speed * Time.deltaTime, Space.Self);
    }

    public void Init(float damage, float speed, EnemyBody enemyBody)
    {
        _damage = damage;
        _speed = speed;
        _enemyBody = enemyBody;
    }
}
