using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";
    
    private float _speed = 1;
    private float _damage;
    private Enemy _enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null && enemy == _enemy)
        {
            enemy.AddDamage(_damage);
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

    public void Init(float damage, float speed, Enemy enemyBody)
    {
        _damage = damage;
        _speed = speed;
        _enemy = enemyBody;
    }
}
