using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnCooldown = 3f;

    [SerializeField] private Transform _enemyParent;
    [SerializeField] private SphereCollider _sphereCollider;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    [ContextMenu("Spawn")]
    public void SpawnEnemy()
    {
        var offset = Random.onUnitSphere;
        offset.y = 0;
        var enemyPos = _sphereCollider.transform.position + offset.normalized*_sphereCollider.radius;

        var newEnemy = Instantiate(_enemyPrefab, enemyPos, Quaternion.identity);
        newEnemy.transform.parent = _enemyParent;
    }

    public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnCooldown);
            SpawnEnemy();
        }
    }
}
