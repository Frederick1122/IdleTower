using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDie;
    
    [SerializeField] private float _hp = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private EnemyBody _enemyBody;
    
    private Tower _tower;
    private bool _isMoving = true;
    private YieldInstruction _halfSecond = new WaitForSeconds(0.5f);
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;

    public EnemyBody GetEnemyBody() => _enemyBody;
    
    public void AddDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Die();
        }
    }
    
    private void Start()
    {
        _tower = GameBus.Instance.GetTower();
        _rigidbody = _enemyBody.GetRigidbody();
        _meshRenderer = _enemyBody.GetMeshRenderer();
        _meshRenderer.enabled = false;
        
        _enemyBody.Init(this);
        _enemyBody.OnCollisionEnterAction += FirstStart;
        StartCoroutine(CheckDistanceRoutine());
    }

    private void FixedUpdate()
    {
        if(_isMoving && _meshRenderer.enabled)
            Move();
    }

    private void Move()
    {
       _rigidbody.velocity = (_tower.transform.position - transform.position).normalized * _speed * Time.fixedDeltaTime;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator CheckDistanceRoutine()
    {
        while (_isMoving)
        {
            yield return _halfSecond;
            if (Vector3.Distance(_enemyBody.transform.position, _tower.transform.position) <= _minDistance)
            {
                _isMoving = false;
                _rigidbody.velocity = Vector3.zero;
                break;                
            }
        }
    }

    private void FirstStart(Collision uselessCollision)
    {
        _meshRenderer.enabled = true;
        _enemyBody.OnCollisionEnterAction -= FirstStart;
    }
}
