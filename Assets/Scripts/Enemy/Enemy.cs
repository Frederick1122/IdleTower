using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    private const float SCAN_DELAY = 0.5f;
    
    public event Action<Enemy> OnDie;
   
    [SerializeField] private float _dieDelay = 0.5f;
    [SerializeField] private float _hp = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private GameObject _skinnedMeshRenderer;
    
    [Inject] private Tower _tower;

    private Rigidbody _rigidbody;
    private EnemyAnimator _animator;
    private bool _isMoving = true;
    private float _costOfEnemyDeathLevel;
    
    private readonly CancellationTokenSource _checkDistanceCts = new();
    private readonly CancellationTokenSource _dieCts = new();

    public void AddDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0) 
            Die();
    }
    
    public Transform GetTargetPoint() => _targetPoint;

    public void Init(float costOfEnemyDeathLevel)
    {
        _costOfEnemyDeathLevel = costOfEnemyDeathLevel;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(!_skinnedMeshRenderer.activeSelf)
            FirstStart();
    }

    private void OnDestroy()
    {
        _checkDistanceCts.Cancel();

        _dieCts.Cancel();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _skinnedMeshRenderer.SetActive(false);
        transform.LookAt(new Vector3(_tower.transform.position.x, transform.position.y, _tower.transform.position.z));

        CheckDistanceTask(_checkDistanceCts.Token).Forget();

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

        _checkDistanceCts.Cancel();
        
        GameBus.Instance.SetCoins(GameBus.Instance.GetCoins() + _costOfEnemyDeathLevel);
        DieTask(_dieCts.Token).Forget();
    }
    
    private void FirstStart()
    {
        _skinnedMeshRenderer.SetActive(true);
    }
    
    private async UniTaskVoid CheckDistanceTask(CancellationToken token)
    {
        while (_isMoving)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(SCAN_DELAY), cancellationToken: token);
            
            if (!(Vector3.Distance(transform.position, _tower.transform.position) <= _minDistance))
                continue;
            
            _isMoving = false;
            _animator.PlayAttack();
            break;
        }
    }
    
    private async UniTaskVoid DieTask(CancellationToken token)
    {
        if (_animator.IsAnimatorActive())
        {
            _animator.PlayDead();
            await UniTask.DelayFrame(1, cancellationToken: token);
            await UniTask.Delay(TimeSpan.FromSeconds(_animator.GetAnimationLength()), cancellationToken: token);
        }

        if (_dieDelay > 0f)
        {
            await transform.DOMoveY(-1, _dieDelay).SetEase(Ease.InCirc).AsyncWaitForCompletion();
        }
        
        Destroy(gameObject);
    }
}
