
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public class EnemyBody : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterAction;
        
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;
        private Enemy _enemy;
        
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterAction?.Invoke(collision);
        }

        public void Init(Enemy enemy)
        {
            _enemy = enemy;
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
        
        public MeshRenderer GetMeshRenderer() => _meshRenderer;

        public Enemy GetEnemy() => _enemy;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
