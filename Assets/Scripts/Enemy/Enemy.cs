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
    private GameObject _skinnedMeshRenderer;
    private EnemyAnimator _animator;

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
        _skinnedMeshRenderer = _enemyBody.GetSkinnedMeshRenderer();
        _skinnedMeshRenderer.SetActive(false);
        
        _enemyBody.Init(this, _tower);
        _enemyBody.OnCollisionEnterAction += FirstStart;
        StartCoroutine(CheckDistanceRoutine());

        _animator = GetComponent<EnemyAnimator>();
    }

    private void FixedUpdate()
    {
        if(_isMoving && _skinnedMeshRenderer.activeInHierarchy)
            Move();
    }

    private void Move()
    {
       _rigidbody.velocity = (_tower.transform.position - transform.position).normalized * _speed * Time.fixedDeltaTime;
       _animator.PlayWalk();
    }

    private void Die()
    {
        OnDie?.Invoke(this);
        _animator.PlayDead();
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
        _skinnedMeshRenderer.SetActive(true);
        _enemyBody.OnCollisionEnterAction -= FirstStart;
    }
}
