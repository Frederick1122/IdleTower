using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDie;
   
    [SerializeField] private float _hp = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private GameObject _skinnedMeshRenderer;
    
    private Tower _tower;
    private bool _isMoving = true;
    private YieldInstruction _halfSecond = new WaitForSeconds(0.5f);
    private Rigidbody _rigidbody;
    private EnemyAnimator _animator;
    private Coroutine _walkRoutine;
    private Coroutine _dieRoutine;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(!_skinnedMeshRenderer.activeSelf)
            FirstStart();
    }

    public void AddDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Die();
        }
    }
    
    public Transform GetTargetPoint() => _targetPoint;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _tower = GameBus.Instance.GetTower();
        _rigidbody = GetComponent<Rigidbody>();
        _skinnedMeshRenderer.SetActive(false);
        transform.LookAt(new Vector3(_tower.transform.position.x, transform.position.y, _tower.transform.position.z));

        _walkRoutine = StartCoroutine(CheckDistanceRoutine());

        _animator = GetComponent<EnemyAnimator>();
    }

    private void FixedUpdate()
    {
        if (!_skinnedMeshRenderer.activeInHierarchy)
            return;
        
        Move();
    }

    private void Move()
    {
        if (_isMoving)
        {
            _rigidbody.velocity = (_tower.transform.position - transform.position).normalized * _speed * Time.fixedDeltaTime;
            _animator.PlayWalk();
        }
        else
            _rigidbody.velocity = Vector3.zero;
    }

    private void Die()
    {
        OnDie?.Invoke(this);
        _isMoving = false;

        if (_walkRoutine != null)
        {
            StopCoroutine(_walkRoutine);
            _walkRoutine = null;
        }
        
        _dieRoutine = StartCoroutine(DieRoutine());
    }
    
    private void FirstStart()
    {
        _skinnedMeshRenderer.SetActive(true);
    }
    
    private IEnumerator CheckDistanceRoutine()
    {
        while (_isMoving)
        {
            yield return _halfSecond;
            
            if (!(Vector3.Distance(transform.position, _tower.transform.position) <= _minDistance))
                continue;
            
            _isMoving = false;
            _animator.PlayAttack();
            break;
        }
    }
    
    private IEnumerator DieRoutine()
    {
        _animator.PlayDead();

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(_animator.GetAnimationLength());
        
        Destroy(gameObject);
    }
}
