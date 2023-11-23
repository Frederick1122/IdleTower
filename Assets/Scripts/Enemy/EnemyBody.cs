
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBody : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterAction;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private GameObject _skinnedMeshRenderer;

        private Rigidbody _rigidbody;
        private Enemy _enemy;
        
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterAction?.Invoke(collision);
        }

        public void Init(Enemy enemy, Tower tower)
        {
            _enemy = enemy;
            transform.LookAt(new Vector3(tower.transform.position.x, transform.position.y, tower.transform.position.z));
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
        
        public GameObject GetSkinnedMeshRenderer() => _skinnedMeshRenderer;

        public Enemy GetEnemy() => _enemy;

        public Transform GetTargetPoint() => _targetPoint;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
